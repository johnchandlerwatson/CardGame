<template id="game">
    <div v-if="model != null" :class="getArenaClass()">
        <div id="card-table" :class="[isLandscape ? 'landscape-card-table' : 'normal-card-table']">
            <div v-if="!isLandscape" id="enemy-cards" class="hand-section-horizontal">
                <div class="card enemy-card" v-for="enemy in model.Game.User2.Hand" :key="enemy.Id">
                    <p>Enemy Card</p>
                </div>               
            </div>
            <div id="enemy-champ-section" class="centered champ-section">
                <div class="username-section">
                    <label for="username">Username: </label>
                    <p>{{model.Game.User2.Username}}</p>
                </div>
                <champion :champ="model.Game.User2.Champion"></champion>
            </div>
            <div id="battlefield" class="even-rows-container">
                <div id="enemy-side" class="even-rows-container">
                    <div id="enemy-side-back" class="flex-row">
                        <div class="played-card" v-for="enemy in model.Game.User2.PlayedBack" :key="enemy.Id">
                            <playedCard :id="enemy.Id" :card="enemy" :class="enemy.Rarity.toLowerCase()" :isEnemy="true"></playedCard>
                        </div>                       
                    </div>
                    <div id="enemy-side-front" class="flex-row">
                        <div class="played-card" v-for="enemy in model.Game.User2.PlayedFront" :key="enemy.Id">
                            <playedCard :id="enemy.Id" :card="enemy" :class="enemy.Rarity.toLowerCase()" :isEnemy="true"></playedCard>
                        </div>
                    </div>
                </div>
                <div id="ally-side" class="even-rows-container ally-side">
                    <div id="ally-side-front" class="flex-row">
                        <div class="played-card" v-for="ally in model.Game.User1.PlayedFront" :key="ally.Id">
                            <playedCard :id="ally.Id" :card="ally" :class="ally.Rarity.toLowerCase()" :isEnemy="false"></playedCard>
                        </div>
                        <div class="played-card">
                            <div class="phantom-card" :class="{ displayed: hoverState.frontHover }"></div>
                        </div>
                    </div>
                    <div id="ally-side-back" class="flex-row">
                        <div class="played-card" v-for="ally in model.Game.User1.PlayedBack" :key="ally.Id">
                            <playedCard :id="ally.Id" :card="ally" :class="ally.Rarity.toLowerCase()" :isEnemy="false"></playedCard>
                        </div>
                        <div class="played-card">
                            <div class="phantom-card" :class="{ displayed: hoverState.backHover }"></div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="ally-champ-section" class="centered champ-section">
                <div class="username-section">
                    <label for="username">Username: </label>
                    <p>{{model.Game.User1.Username}}</p>
                </div>
                <champion :champ="model.Game.User1.Champion"></champion>
            </div>
            <handcards ref="handcards" v-on:hoverStateChange="updateHoverState" v-on:selectedCard="selectCard" v-if="!isLandscape" :model="model" :hello-model="helloModel" :disabled="disabled" :vertical="false"></handcards>
        </div>
        <div v-if="!isMobile" id="actions" class="action-section">
            <h4 class="centered">Actions</h4>
            <p class="actions-summary scrollable">{{model.MoveSummary}}</p>
        </div>
        <handcards ref="handcards" v-on:hoverStateChange="updateHoverState" v-on:selectedCard="selectCard" :model="model" :hello-model="helloModel" :vertical="true" :disabled="disabled" v-else-if="isMobile && isLandscape"></handcards>
        <disconnected v-on:goToLobby="goToLobby()" v-if="enemyLeft"></disconnected>
        <gameover v-on:goToLobby="goToLobby()" :model="model" :helloModel="helloModel"></gameover>  
    </div>
</template>

<script>
  var signalR = require('../js/signalr.js').signalR
  import playedCard from './PlayedCard.vue'
  import gameover from './GameOver.vue'
  import champion from './Champion.vue'
  import disconnected from './Disconnected.vue'
  import handcards from './HandCards.vue'

  export default {
    name: 'game',
    props: ['helloModel'],
    components: { playedCard, gameover, champion, disconnected, handcards },
    data () {
      return {
        model: null,
        disabled: false,
        enemyLeft: false,
        isLandscape: null,
        hoverState: {
          frontHover: false,
          backHover: false
        }
      }
    },
    created () {
      connectSignalR(this)
      var url = `/api/Game/${this.helloModel.username}/${this.helloModel.deckName}/${this.helloModel.champName}/${this.helloModel.gameId}`
      this.$http
        .get(url)
        .then((res) => {
          this.model = res.body
        })
      .catch((ex) => console.log(ex))

      // watch for orientation changes
      var vue = this
      checkOrientation(vue)
      window.onorientationchange = function () {
        vue.isLandscape = !vue.isLandscape
      }

      function checkOrientation (vue) {
        vue.isLandscape = window.innerWidth > window.innerHeight && vue.isMobile
      }

      function connectSignalR (vue) {
        vue.connection = new signalR.HubConnection('/gamelobby', { logger: signalR.LogLevel.Error })
        vue.connection.on('EndTurn', (gameId) => {
          if (gameId === vue.helloModel.gameId) {
            var url = `/api/Game/${vue.helloModel.gameId}/${vue.helloModel.username}`
            vue.$http
              .get(url)
              .then((res) => {
                vue.model = res.body
                vue.disabled = false
              })
              .catch((ex) => console.log(ex))
            vue.$refs.handcards.resetTimer()
          }
        })
        vue.connection.on('PlayerLeft', (gameId) => {
          if (gameId === vue.helloModel.gameId) {
            vue.enemyLeft = true
            vue.helloModel.gameId = null
          }
        })
        vue.connection.start().catch(err => console.log(err))
      }
    },
    methods: {
      selectCard: function (selection) {
        var userPair = { User1: this.model.Game.User1, User2: this.model.Game.User2 }
        var payload = { Selection: selection, UserPair: userPair, Username: this.helloModel.username, GameId: this.helloModel.gameId }
        this.$http.post('/api/Game/', JSON.stringify(payload)).then((response) => {
          if (response.body.PlayerGame) {
            this.$data.model.Game.User1 = response.body.User
            this.disabled = true
          } else {
            this.$data.model = response.body
          }
        }).catch((ex) => console.log(ex))
      },
      showAction: function (actionCard, action) {
        var owner = action.Actor.User.Username
        var attackedCard = document.getElementById(action.DamagedCharacters[0].Id)
        console.log(owner + '\'s ' + action.Actor.Name + ' attacking ' + attackedCard.id)
        if (owner === this.model.Game.User1.Username) {
          actionCard.classList.add('attack-enemy')
        } else {
          actionCard.classList.add('attack-ally')
        }
      },
      goToLobby: function () {
        this.$emit('input', { componentName: 'gameLobby', deckName: this.helloModel.deckName, champName: this.helloModel.champName, username: this.helloModel.username })
      },
      updateHoverState: function (hoverState) {
        this.hoverState = hoverState
      },
      getArenaClass: function () {
        if (this.isMobile) {
          if (this.isLandscape) {
            return 'mobile-landscape-arena'
          } else {
            return 'mobile-portrait-arena'
          }
        } else {
          return 'desktop-arena'
        }
      }
    },
    watch: {
      'model.Game.Actions': function (actions, previousActions) {
        if (actions.length > 0) {
          console.log(actions)
          for (var i = previousActions.length; i < actions.length; i++) {
            var action = actions[i]
            console.log('new action')
            if (action.Actor) {
              var actionCard = document.getElementById(action.Actor.Id)
              this.showAction(actionCard, action)
            } else {
              console.log('no Actor')
            }
          }
        }
      }
    }
  }
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
    body {
        padding: 0;
    }

    .normal-card-table {
        display: grid;
        grid-template-rows: 10% 10% 50% 10% 20%
    }

    .landscape-card-table {
        display: grid;
        grid-template-rows: 20% 60% 20%;
    }

    .attack-enemy { 
        animation: attack-enemy 0.4s both;
        position: relative;
        animation-delay: .5s;
    }

    .attack-ally { 
        animation: attack-ally 0.4s both;
        position: relative;
        animation-delay: .5s;
    }

    @keyframes attack-enemy {
        0%   {left:0px; top:0px;}
        50%  {left:0px; top:-50px;}
        75%  {left:0px; top:-75px;}
        100% {left:0px; top:0px;}
    }

    @keyframes attack-ally {
        0%   {left:0px; top:0px;}
        50%  {left:0px; top:50px;}
        75%  {left:0px; top:75px;}
        100% {left:0px; top:0px;}
    }

    .desktop-arena {
        display: grid;
        grid-template-columns: 85% 15%;
        height: 100%;
        overflow-x: hidden;
    }

    .mobile-portrait-arena {
        display: grid;
        grid-template-columns: 100%;
        height: 100%;
        overflow-x: hidden;
    }

    .mobile-landscape-arena {
        display: grid;
        grid-template-columns: 80% 20%;
        height: 100%;
        overflow-x: hidden;
    }

    .even-rows-container {
        display: grid;
        grid-template-rows: 50% 50%;
    }

    .ally-side {
        border-top: #655539 dashed;
    }

    .flex-row {
        display: flex; 
        flex-direction: row;
        justify-content: center;
        overflow: hidden; 
    }

    .champ-section {
        background-color: #948da0;
        border: #514863 solid;
        display: grid;
        grid-template-columns: 10% 60%;
        grid-column-gap: 10%;
    }

    .enemy-card {
        font-size: .6em;
    }

    .username-section {
        margin-top: 10px;
        margin-left: 10px;
    }

    .action-section {
        display: grid;
        grid-template-rows: 5% 85% 10%;
        height: 100%;
        background-color: white;  
    }

    .actions-summary
    {
        font-size: .8em;
        padding: 12px;
        white-space: pre-line;
        width: 15%;
        text-overflow: ellipsis;
        overflow-x: hidden;
    }

    .scrollable {
        height: 100%;
        overflow-y: auto;
        top: 50px;
        position: absolute;
    }

    #battlefield {
        background: radial-gradient(#ffe6b9 60%, #ab9469 105%);
    }

    .displayed {
        display: block !important;
    }

    .phantom-card {
        display: none;
        background: radial-gradient(#828181, #cac6bd);
        opacity: .8;
        margin: 10px;
        border-radius: 4px;
        padding: 10px;
        width: 125px;
        height: 50px;
        -webkit-filter: blur(1.5px);
        -moz-filter: blur(1.5px);
        -o-filter: blur(1.5px);
        -ms-filter: blur(1.5px);
        filter: blur(1.5px);
    }
</style>