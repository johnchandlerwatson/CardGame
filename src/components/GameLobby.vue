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
      this.$http
        .get('/api/GameLobby')
        .then((res) => {
          console.log(res)
          this.model = res.body
        })
        .catch((ex) => console.log(ex))
    },
    methods: {
      joinGame: function () {
        this.$http
        .get('/api/GameLobby/JoinGame' + this.helloModel.username + '/' + this.helloModel.deckName + '/' + this.helloModel.champName)
        .then((res) => {
          console.log(res)
          this.joinGameModel = res.body
        })
        .catch((ex) => console.log(ex))

        if (this.joinGameModel.Full) {
          this.goToArena()
        }
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