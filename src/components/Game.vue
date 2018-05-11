<template id="game">
    <div v-if="model != null" class="arena">
        <div id="card-table">
            <div id="enemy-cards" style="height: 10%;" class="hand-section">
                <div class="card enemy-card" v-for="enemy in model.Game.User2.Hand" :key="enemy.Id">
                    <p>Enemy Card</p>
                </div>               
            </div>
            <div id="enemy-champ-section" class="centered champ-section">
                <div>
                    <img class="champ" :src="getImage(model.Game.User2.Champion.ChampImage)">
                    <p>HLTH: {{model.Game.User2.Champion.Health}}</p>
                </div>
            </div>
            <drop id="battlefield" style="height: 50%" class="drop even-rows-container" @drop="handleDrop">
                <div id="enemy-side" class="even-rows-container">
                    <div id="enemy-side-back" class="flex-row">
                        <div class="played-card" v-for="enemy in model.Game.User2.PlayedBack" :key="enemy.Id">
                            <playedCard :card="enemy" :class="enemy.Rarity.toLowerCase()" :isEnemy="true"></playedCard>
                        </div>                       
                    </div>
                    <div id="enemy-side-front" class="flex-row">
                        <div class="played-card" v-for="enemy in model.Game.User2.PlayedFront" :key="enemy.Id">
                            <playedCard :card="enemy" :class="enemy.Rarity.toLowerCase()" :isEnemy="true"></playedCard>
                        </div>
                    </div>
                </div>
                <div id="ally-side" class="even-rows-container" style="border-top: #655539 dashed;">
                    <div id="ally-side-front" class="flex-row">
                        <div class="played-card" v-for="ally in model.Game.User1.PlayedFront" :key="ally.Id">
                            <playedCard :card="ally" :class="ally.Rarity.toLowerCase()" :isEnemy="false"></playedCard>
                        </div>
                        <div class="played-card">
                            <div class="phantom-card" :class="{ displayed: dragging === 'Front' }"></div>
                        </div>
                    </div>
                    <div id="ally-side-back" class="flex-row">
                        <div class="played-card" v-for="ally in model.Game.User1.PlayedBack" :key="ally.Id">
                            <playedCard :card="ally" :class="ally.Rarity.toLowerCase()" :isEnemy="false"></playedCard>
                        </div>
                        <div class="played-card">
                            <div class="phantom-card" :class="{ displayed: dragging === 'Back' }"></div>
                        </div>
                    </div>
                </div>
            </drop>
            <div id="ally-champ-section" class="centered champ-section">
                <div>
                    <img class="champ" :src="getImage(model.Game.User1.Champion.ChampImage)">
                    <p>HLTH: {{model.Game.User1.Champion.Health}}</p>
                </div>
            </div>
            <div id="ally-cards" style="height: 20%;" class="hand-section">
                <drag class="drag card ally-card" :class="ally.Rarity.toLowerCase()" @dragstart="dragging = ally.Row" @dragend="dragging = null" v-for="ally in model.Game.User1.Hand" :transfer-data="{ CardName: ally.Name }" :key="ally.Id">
                    <span>{{ally.Name}}</span><br>
                    <span>Health: {{ally.Health}}</span><br>
                    <span>Damage: {{ally.Damage}}</span><br>
                    <span>Rarity: {{ally.Rarity}}</span><br>
                    <span>Side: {{ally.Row}}</span><br>
                </drag>               
                <input type="text" id="card-selected" hidden>
            </div>
        </div>
        <div id="actions" class="action-section">
            <h4 class="centered">Actions</h4>
            <p class="actions-summary scrollable">{{model.MoveSummary}}</p>
        </div>
        <gameover :model="model"></gameover>
    </div>
</template>

<script>
  var signalR = require('../js/signalr.js').signalR
  import { Drag, Drop } from 'vue-drag-drop'
  import playedCard from './PlayedCard.vue'
  import gameover from './GameOver.vue'

  export default {
    name: 'game',
    props: ['helloModel'],
    components: { Drag, Drop, playedCard, gameover },
    data () {
      return {
        model: null,
        dragging: null
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
              })
              .catch((ex) => console.log(ex))
          }
        })
        vue.connection.start().catch(err => console.log(err))
      }
    },
    methods: {
      selectCard: function (selection) {
        event.preventDefault()
        var userPair = { User1: this.$data.model.Game.User1, User2: this.$data.model.Game.User2 }
        var payload = { Selection: selection, UserPair: userPair, Username: this.helloModel.username, GameId: this.helloModel.gameId }
        this.$http.post('/api/Game/', JSON.stringify(payload)).then((response) => {
          if (response.body.PlayerGame) {
            this.$data.model.Game.User1 = response.body.User
          } else {
            this.$data.model = response.body
          }
        }).catch((ex) => console.log(ex))
      },
      handleDrop: function (data) {
        this.selectCard(data.CardName)
      },
      findDrop: function (data) {
        console.log(data)
      },
      getImage: function (picName) {
        var images = require.context('../assets/', false, /\.png$/)
        return images('./' + picName + '.png')
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

    .rare{
        background: linear-gradient(to right, #c72424 , #504d4d) !important
    }

    .arena {
        display: grid;
        grid-template-columns: 85% 15%;
        height: 100%;
        overflow-x: hidden;
    }

    .even-rows-container {
        display: grid;
        grid-template-rows: 50% 50%;
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
    }

    .champ {
        width: 25px;
    }

    .ally-card {
        width: 150px;
        cursor: pointer;
        font-size: .8em;
        padding-left: 20px;
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
        background-color: #dfd;
        margin: 10px;
        border-radius: 4px;
        padding: 10px;
        width: 125px;
        height: 50px;
    }
</style>