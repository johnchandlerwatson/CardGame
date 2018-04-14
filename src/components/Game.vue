<template id="game">
    <div v-if="model != null" class="arena">
        <div id="card-table">
            <div id="enemy-cards" style="height: 10%;" class="hand-section">
                <div class="card enemy-card" v-for="enemy in model.Enemy.Hand" v-bind:key="enemy.Id">
                    <p>Enemy Card</p>
                </div>               
            </div>
            <div id="enemy-champ-section" class="centered champ-section">
                Enemy Champ Section
            </div>
            <drop id="battlefield" style="height: 50%" class="drop even-rows-container" @drop="handleDrop">
                <div id="enemy-side" class="even-rows-container">
                    <div id="enemy-side-back" class="flex-row">
                        <div class="played-card" v-for="enemy in model.Enemy.PlayedBack" v-bind:key="enemy.Id">
                            <span>{{enemy.Name}}</span><br>
                            <span>Health: {{enemy.Health}}</span><br>
                        </div>                       
                    </div>
                    <div id="enemy-side-front" class="flex-row">
                        <div class="played-card" v-for="enemy in model.Enemy.PlayedFront" v-bind:key="enemy.Id">
                            <span>{{enemy.Name}}</span><br>
                            <span>Health: {{enemy.Health}}</span><br>
                        </div>
                    </div>
                </div>
                <div id="ally-side" class="even-rows-container" style="border-top: #655539 dashed;">
                    <div id="ally-side-front" class="flex-row">
                        <div class="played-card" v-for="ally in model.User.PlayedFront" v-bind:key="ally.Id">
                            <span>{{ally.Name}}</span><br>
                            <span>Health: {{ally.Health}}</span><br>
                        </div>
                    </div>
                    <div id="ally-side-back" class="flex-row">
                        <div class="played-card" v-for="ally in model.User.PlayedBack" v-bind:key="ally.Id">
                            <span>{{ally.Name}}</span><br>
                            <span>Health: {{ally.Health}}</span><br>
                        </div>
                    </div>
                </div>
            </drop>
            <div id="ally-champ-section" class="centered champ-section">
                Ally Champ Section
            </div>
            <div id="ally-cards" style="height: 20%;" class="hand-section">
                <drag class="drag card ally-card" v-for="ally in model.User.Hand" :transfer-data="{ CardName: ally.Name }" v-bind:key="ally.Id">
                    <span>{{ally.Name}}</span><br>
                    <span>Health: {{ally.Health}}</span><br>
                    <span>Damage: {{ally.Damage}}</span><br>
                    <span>Rarity: {{ally.Rarity}}</span><br>
                </drag>               
                <input type="text" id="card-selected" hidden>
            </div>
        </div>
        <div id="actions" class="action-section">
            <h4 class="centered">Actions</h4>
            <p class="actions-summary scrollable">{{model.MoveSummary}}</p>
        </div>
    </div>
</template>

<script>
  import $ from 'jquery'
  import { Drag, Drop } from 'vue-drag-drop'

  export default {
    name: 'game',
    props: ['helloModel'],
    components: { Drag, Drop },
    data () {
      return {
        model: null
      }
    },
    created () {
      var username = $('#username').val()
      debugger
      this.$http
          .get('/api/Game/' + username + '/' + this.helloModel.deckName + '/' + this.helloModel.champName)
          .then((res) => {
            this.model = res.body
          })
          .catch((ex) => console.log(ex))
    },
    methods: {
      selectCard: function (selection) {
        event.preventDefault()
        var enemy = this.$data.model.EndOfTurnModel.Enemy
        var user = this.$data.model.EndOfTurnModel.User
        var payload = { Selection: selection, Enemy: enemy, User: user }
        this.$http.post('/api/Game/', JSON.stringify(payload)).then((response) => {
          this.$data.model = response.body
        }, (response) => {
          console.log('there was an error getting the move response')
        })
      },
      handleDrop: function (data) {
        this.selectCard(data.CardName)
      }
    }
  }
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
    body {
        padding: 0;
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

    .centered {
        display: grid;
        align-content: center;
        align-items: center;
        text-align: center;
    }

    .champ-section {
        height: 10%; 
        background-color: #948da0;
        border: #514863 solid;
    }

    .ally-card {
        width: 150px;
        cursor: pointer;
        font-size: .8em;
        padding-left: 20px;
    }

    .enemy-card {
        font-size: .6em;
        width: 50px;
        height: 50px;
    }

    .played-card {
        background: linear-gradient(to right, #6b5d5d , #504d4d);
        color: #f3eeee;
        margin: 10px;
        border-radius: 4px;
        padding: 10px;
        width: 125px;
        font-size: .8em;
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
</style>