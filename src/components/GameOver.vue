<template id="gameover">
    <transition name="fade">
        <div v-show="gameIsOver" class="modal-backdrop">
            <div class="basic-modal game-over-modal">
                <div class="centered">
                    <img class="gif" :src="getImage()">
                    <h1 style="font-size: 4em;">{{gameOverMessage}}</h1>
                    <div class="row">
                        <button v-on:click="returnToLobby" class="btn btn-lg btn-success">Return</button>
                    </div>
                </div>   
            </div>
        </div>
    </transition>
</template>

<script>
  export default {
    name: 'gameover',
    props: ['model', 'helloModel'],
    computed: {
      gameIsOver: function () {
        return this.lose || this.win
      },
      win: function () {
        return this.model.Game.User2.Champion.Health <= 0
      },
      lose: function () {
        return this.model.Game.User1.Champion.Health <= 0
      },
      gameOverMessage: function () {
        if (this.win) {
          return 'YOU WIN!!!'
        } else if (this.lose) {
          return 'YOU SUCK!!!'
        } else {
          return 'GAME OVER!'
        }
      }
    },
    methods: {
      getImage: function () {
        var picName = this.win ? 'balloons' : 'skull'
        var images = require.context('../assets/', false, /\.gif$/)
        return images('./' + picName + '.gif')
      },
      returnToLobby: function () {
        this.$emit('goToLobby')
      }
    }
  }
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
    .game-over-modal {
        width: 70%;
        height: 40%;
        max-width: 650px;
    }

    .gif {
        z-index: -1;
        opacity: .4;
        position: absolute;
        width: 100%;
        height: 250%;
        top: -210px;
    }
</style>