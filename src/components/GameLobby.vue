<template id="gameLobby">
  <div class="container">
      <h1>Game Lobby</h1>
      <p>Current Games: {{model.CurrentGameCount}}</p>
      <button @click="joinGame()">Multiplayer Game</button>
      <button @click="goToArena()">Bot Game</button>
      <img class="loading" v-show="(this.joinGameModel.Id != '')" src="../assets/loading.gif">
      <p>{{joinGameModel.Id}}</p>
  </div>
</template>

<script>
  var signalR = require('../js/signalr.js').signalR
  export default {
    name: 'gameLobby',
    props: ['helloModel'],
    data () {
      return {
        model: { CurrentGameCount: 0 },
        joinGameModel: { Id: '' }
      }
    },
    created () {
      connectSignalR(this)

      this.$http
        .get('/api/GameLobby')
        .then((res) => {
          this.model = res.body
        })
        .catch((ex) => console.log(ex))

      function connectSignalR (vue) {
        vue.connection = new signalR.HubConnection('/gamelobby', { logger: signalR.LogLevel.Error })
        vue.connection.on('NewGame', (gameId) => {
          if (gameId === vue.joinGameModel.Id) {
            vue.goToArena()
          }
        })
        vue.connection.start().catch(err => console.log(err))
      }
    },
    methods: {
      joinGame: function () {
        var url = '/api/GameLobby/' + this.helloModel.username + '/' + this.helloModel.deckName + '/' + this.helloModel.champName
        this.$http
        .get(url)
        .then((res) => {
          this.joinGameModel = res.body
          if (this.joinGameModel.Full) {
            this.$http.get('/api/GameLobby/' + this.joinGameModel.Id)
              .catch(err => console.log(err))
            this.goToArena()
          }
        })
        .catch((ex) => console.log(ex))
      },
      goToArena: function () {
        this.$emit('input', { componentName: 'game', deckName: this.helloModel.deckName, champName: this.helloModel.champName, username: this.helloModel.username, gameId: this.joinGameModel.Id })
      }
    }
  }
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
    .loading {
        width: 4%;
    }
</style>