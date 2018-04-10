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
            <div id="battlefield" style="height: 50%" class="even-rows-container">
                <div id="enemy-side" class="even-rows-container" style="background-color: rgb(236, 204, 204);">
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
                <div id="ally-side" class="even-rows-container" style="background-color: rgb(203, 233, 203);">
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
            </div>
            <div id="ally-champ-section" class="centered champ-section">
                Ally Champ Section
            </div>
            <div id="ally-cards" style="height: 20%;" class="hand-section">
                <div class="card ally-card" v-for="ally in model.User.Hand" @click="selectCard(ally.Name)" v-bind:key="ally.Id">
                    <span>{{ally.Name}}</span><br>
                    <span>Health: {{ally.Health}}</span><br>
                    <span>Damage: {{ally.Damage}}</span><br>
                    <span>Rarity: {{ally.Rarity}}</span><br>
                </div>               
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

  export default {
    name: 'game',
    props: {
      'username': {
        type: String
      }
    },
    data () {
      return {
        model: null
      }
    },
    created () {
      var username = $('#username').val()
      this.$http
          .get('/api/Game/' + username)
          .then((res) => {
            this.model = res.body
          })
          .catch((ex) => console.log(ex))
    },
    methods: {
      selectCard: function (selection) {
        event.preventDefault()
        var enemyCards = this.$data.model.EndOfTurnModel.EnemyPlayedCards
        var allyCards = this.$data.model.EndOfTurnModel.UserPlayedCards
        var payload = { Selection: selection, EnemyPlayedCards: enemyCards, UserPlayedCards: allyCards }
        this.$http.post('/api/Game/', JSON.stringify(payload)).then((response) => {
          this.$data.model = response.body
        }, (response) => {
          console.log('there was an error getting the move response')
        })
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
    }

    .even-rows-container {
        display: grid;
        grid-template-rows: 50% 50%;
    }

    .hand-section {
        display: flex; 
        flex-direction: row;
        background-color: rgb(204, 204, 228)
    }

    .flex-row {
        display: flex; 
        flex-direction: row;
    }

    .centered {
        display: grid;
        align-content: center;
        align-items: center;
        text-align: center;
    }

    .champ-section {
        height: 10%; 
        background-color: rgb(255, 245, 190)
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
        background: #b9a5a5;
        margin: 10px;
        border-radius: 4px;
        padding: 10px;
    }

    .card {
        padding: 10px;
        background-color: #af9e9e;
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
    }

    .scrollable {
        height: 100%;
        overflow-y: auto;
        top: 50px;
        position: absolute;
    }
</style>