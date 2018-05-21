<template id="game">
    <div v-if="model != null" :class="[isMobile ? 'mobile-arena' : 'desktop-arena']">
        <div id="card-table">
            <div id="enemy-cards" style="height: 10%;" class="hand-section">
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
            <div id="battlefield" style="height: 50%" class="even-rows-container">
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
                            <div class="phantom-card" :class="{ displayed: frontHover }"></div>
                        </div>
                    </div>
                    <div id="ally-side-back" class="flex-row">
                        <div class="played-card" v-for="ally in model.Game.User1.PlayedBack" :key="ally.Id">
                            <playedCard :id="ally.Id" :card="ally" :class="ally.Rarity.toLowerCase()" :isEnemy="false"></playedCard>
                        </div>
                        <div class="played-card">
                            <div class="phantom-card" :class="{ displayed: backHover }"></div>
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
            <div id="ally-cards" style="height: 20%;" class="hand-section" :class="{'disabled': disabled}">
                <div @click="selectCard(ally.Name)" @mouseover="onHover(ally.Row)" @mouseleave="offHover()" class="card ally-card" :class="ally.Rarity.toLowerCase()" v-for="ally in model.Game.User1.Hand" :key="ally.Id">
                    <span>{{ally.Name}}</span><br>
                    <span>Health: {{ally.Health}}</span><br>
                    <span>Damage: {{ally.Damage}}</span><br>
                    <span>Rarity: {{ally.Rarity}}</span><br>
                    <span>Side: {{ally.Row}}</span><br>
                </div>               
                <input type="text" id="card-selected" hidden>
                <timer ref="timer" v-if="!isBotGame()" v-on:forceTurn="selectCard(firstCard())"></timer>
            </div>
            <div v-if="disabled" class="disabled-overlay" id="disable-section">
                <h2>Waiting for oponent's move..</h2>
            </div>
        </div>
        <div v-if="!isMobile" id="actions" class="action-section">
            <h4 class="centered">Actions</h4>
            <p class="actions-summary scrollable">{{model.MoveSummary}}</p>
        </div>
        <disconnected v-on:goToLobby="goToLobby()" v-if="enemyLeft"></disconnected>
        <gameover v-on:goToLobby="goToLobby()" :model="model" :helloModel="helloModel"></gameover>  
    </div>
</template>

<script>
  var signalR = require('../js/signalr.js').signalR
  import playedCard from './PlayedCard.vue'
  import gameover from './GameOver.vue'
  import timer from './Timer.vue'
  import champion from './Champion.vue'
  import disconnected from './Disconnected.vue'

  export default {
    name: 'game',
    props: ['helloModel'],
    components: { playedCard, gameover, timer, champion, disconnected },
    data () {
      return {
        model: null,
        dragging: null,
        disabled: false,
        enemyLeft: false,
        frontHover: false,
        backHover: false
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
            vue.$refs.timer.resetTimer()
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
        this.frontHover = false
        this.backHover = false
        var userPair = { User1: this.$data.model.Game.User1, User2: this.$data.model.Game.User2 }
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
      showAction: function (actionCard, owner, cardName) {
        console.log(owner + '\'s ' + cardName + ' attacking someone')
        if (owner === this.model.Game.User1.Username) {
          actionCard.classList.add('attack-enemy')
        } else {
          actionCard.classList.add('attack-ally')
        }
      },
      goToLobby: function () {
        this.$emit('input', { componentName: 'gameLobby', deckName: this.helloModel.deckName, champName: this.helloModel.champName, username: this.helloModel.username })
      },
      firstCard: function () {
        return this.$data.model.Game.User1.Hand[0].Name
      },
      isBotGame: function () {
        return this.helloModel.gameId === null
      },
      onHover: function (row) {
        if (row === 'Front') {
          this.frontHover = true
        } else {
          this.backHover = true
        }
      },
      offHover: function () {
        this.frontHover = false
        this.backHover = false
      }
    },
    watch: {
      'model.Game.Actions': function (val, oldVal) {
        if (val.length > 0) {
          console.log(val)
          for (var i = oldVal.length; i < val.length; i++) {
            console.log('new action')
            if (val[i].Actor) {
              var actionCard = document.getElementById(val[i].Actor.Id)
              this.showAction(actionCard, val[i].Actor.User.Username, val[i].Actor.Name)
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

    /* Common - Grey Uncommon - Red Rare - Orange Epic - Green Legendary - Blue Mythic - Purple Godly - Gold */
    .common{
        background: linear-gradient(to right, #3e3e41 , #504d4d) !important
    }

    .uncommon{
        background: linear-gradient(to right, #c72424 , #5f0e0e) !important
    }

    .rare{
        background: linear-gradient(to right, #dd9613 , #5c2908) !important
    }

    .epic{
        background: linear-gradient(to right, #266430 , #023006) !important
    }

    .legendary{
        background: linear-gradient(to right, #1c2aec , #06012b) !important
    }

    .mythic{
        background: linear-gradient(to right, #561d7c , #1e0225) !important
    }

    .godly{
        background: linear-gradient(to right, #d89924 , #6b5c1b) !important
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

    .mobile-arena {
        display: grid;
        grid-template-columns: 100%;
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

    .hand-section {
        display: flex; 
        flex-direction: row;
        background-color: rgb(156, 156, 162);
    }

    .flex-row {
        display: flex; 
        flex-direction: row;
        justify-content: center;
        overflow: hidden; 
    }

    .champ-section {
        height: 10%; 
        background-color: #948da0;
        border: #514863 solid;
        display: grid;
        grid-template-columns: 10% 60%;
        grid-column-gap: 10%;
    }

    .ally-card {
        width: 150px;
        cursor: pointer;
        font-size: .8em;
        padding-left: 20px;
    }

        .ally-card:hover {
            border: #c3dbdb solid 4px;
        }

    .enemy-card {
        font-size: .6em;
    }

    .card {
        padding: 10px;
        background: linear-gradient(to right, #6b5d5d , #504d4d);
        color: #f3eeee;
        margin: 10px;
        border-radius: 10px;
        box-shadow: black 2px 2px 4px 0px;
    }

    .username-section {
        margin-top: 10px;
        margin-left: 10px;
    }

    .selected {
        background-color: #bbaaaa;
        border-color: #a5a5de;
        border-width: 5px;
        border-style: groove;
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
        background-color: rgb(202, 218, 202);
        margin: 10px;
        border-radius: 4px;
        padding: 10px;
        width: 125px;
        height: 50px;
    }

    .disabled-overlay {
        opacity: 0.7;
        background: #626263;
        z-index: 1000;
        position: absolute;
        bottom: 0;
        height: 20%;
        width: 85%;
        color: white;
        text-align: center;
        font-size: 2em;
        padding-top: 20px;
    }

    .disabled {
        pointer-events: none;
    }
</style>