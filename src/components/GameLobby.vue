<template id="gameLobby">
  <div class="container game-lobby">
      <div id="top-area">
        <h2>Game Lobby</h2>
        <p>Current Games: {{model.CurrentGameCount}}</p>
        <button class="btn btn-primary" @click="joinGame()">Multiplayer Game</button>
        <button class="btn btn-primary" @click="goToArena()">Bot Game</button>
        <img class="loading" v-show="(this.joinGameModel.Id != '')" src="../assets/loading.gif">

        <div id="chat-area" class="centered">
          <h3>Chat</h3>
          <form id="chat-input" v-on:submit.prevent="submitChat">
            <input class="chat-input" type="text" v-model="chat">
            <button @click="submitChat" id="submit-chat" class="btn btn-success">Submit</button>
          </form>
        </div>
      </div>
      <div class="messages">
        <p v-for="message in model.Messages" :key="message.Id">{{message}}</p>
      </div>
  </div>
</template>

<script>
  var signalR = require('../js/signalr.js').signalR
  export default {
    name: 'gameLobby',
    props: ['helloModel'],
    data () {
      return {
        model: { CurrentGameCount: 0, Messages: [] },
        joinGameModel: { Id: '' },
        chat: ''
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
        vue.connection.on('ReceiveMessage', (message) => {
          var msg = message.replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;')
          vue.model.Messages.unshift(msg)
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
          this.connection.invoke('Subscribe', this.joinGameModel.Id).catch(err => console.error(err.toString()))
          if (this.joinGameModel.Full) {
            this.$http.get('/api/GameLobby/' + this.joinGameModel.Id)
              .catch(err => console.log(err))
            this.goToArena()
          }
        })
        .catch((ex) => console.log(ex))
      },
      goToArena: function () {
        var gameId = this.joinGameModel.Id === '' ? null : this.joinGameModel.Id
        this.$emit('input', { componentName: 'game', deckName: this.helloModel.deckName, champName: this.helloModel.champName, username: this.helloModel.username, gameId: gameId })
      },
      submitChat: function () {
        if (this.chat !== '') {
          this.connection.invoke('SendChatMessage', this.helloModel.username, this.chat).catch(err => console.error(err.toString()))
          this.chat = ''
        }
      }
    }
  }
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
    .game-lobby {
      display: grid;
      grid-template-rows: 0fr 40% 60%;
      height: 100%;
    }

    .loading {
        width: 4%;
    }

    .messages {
      background: #e0e0e0;
      border-radius: 7px;
      padding: 10px;
      margin-top: 10px;
      overflow-y: auto;
      height: 95%;
    }

    .chat-input {
      border: #797979 solid 1px;
      border-radius: 4px;
    }
</style>