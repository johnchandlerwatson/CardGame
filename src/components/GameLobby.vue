<template id="gameLobby">
  <div class="container game-lobby">
      <div id="top-area">
        <h2>Game Lobby</h2>
        <p>Current Games: {{model.CurrentGameCount}}</p>
        <button class="btn btn-primary" @click="joinGame()">Multiplayer Game</button>
        <button class="btn btn-primary" @click="goToArena()">Bot Game</button>
        <img class="loading" v-show="(this.joinGameModel.Id != '')" src="../assets/loading.gif">
      </div>
      <div id="bottom-area">
        <div id="chat-area" class="centered">
          <h3>Chat</h3>
          <form id="chat-input" v-on:submit.prevent="submitChat">
            <input class="chat-input" type="text" v-model="chat">
            <button @click="submitChat" id="submit-chat" class="btn btn-success">Submit</button>
          </form>
        </div>
        <div class="messages">
          <div class="message" v-for="chatMessage in model.Messages" :key="chatMessage.Id">
            <span class="chat-text">{{chatMessage.message}}</span>
            <span class="date-sent">{{formatDate(chatMessage.date)}}</span>
          </div>
        </div>
      </div>
  </div>
</template>

<script>
  var signalR = require('../js/signalr.js').signalR
  var moment = require('moment')
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

      var offset = new Date().getTimezoneOffset()
      this.$http
        .get('/api/GameLobby/Index/' + offset)
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
          vue.model.Messages.unshift(message)
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
            this.$http.get('/api/GameLobby/AlertClients/' + this.joinGameModel.Id)
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
      },
      formatDate: function (date) {
        if (date === undefined) {
          return ''
        }
        // strip off timezone
        if (date.indexOf('Z') > 0) {
          date = date.substring(0, date.indexOf('Z'))
        }
        var appliedTimeZone = moment.utc(date).local().calendar()
        return appliedTimeZone
      }
    }
  }
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
    .game-lobby {
      display: grid;
      grid-template-rows: 0fr 25% 60%;
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
      overflow-x: hidden;
      width: 100%;
      height: 95%;
    }

    .message {
      display: grid;
      grid-template-columns: 75% 25%;
    }

    .date-sent {
      font-size: .7em; 
      text-align: right;
      color: #6d6969;
    }

    .chat-input {
      border: #797979 solid 1px;
      border-radius: 4px;
    }

    .chat-text {    
      background: #e5e8fb8f;
      width: max-content;
      max-width: 100%;
      border-radius: 5px;
      padding-left: 7px;
      padding-right: 7px;
      padding-top: 2px;
      padding-bottom: 2px;
      margin-bottom: 8px;
      box-shadow: #a5a7c1bd 0.5px 0.5px 1px 1px;
    }
</style>