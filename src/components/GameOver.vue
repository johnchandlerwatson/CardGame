<template id="gameover">
    <transition name="fade">
        <div v-show="showModal" class="modal-backdrop">
            <div class="basic-modal game-over-modal">
                <div class="centered">
                    <h1 style="font-size: 4em;">{{gameOverMessage}}</h1>
                </div>   
            </div>
        </div>
    </transition>
</template>

<script>
  export default {
    name: 'gameover',
    props: ['model'],
    computed: {
      showModal: function () {
        var show = this.model.User.Champion.Health <= 0 || this.model.Enemy.Champion.Health <= 0
        if (show) {
          this.goHome()
        }
        return show
      },
      gameOverMessage: function () {
        if (this.model.Enemy.Champion.Health <= 0) {
          return 'YOU WIN!!!'
        } else if (this.model.User.Champion.Health <= 0) {
          return 'YOU SUCK!!!'
        } else {
          return 'GAME OVER!'
        }
      }
    },
    methods: {
      goHome: async function () {
        setTimeout(() => {
          this.$router.go()
        }, 2000)
      }
    }
  }
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
    .game-over-modal {
        width: 70%;
        height: 40%;
    }
</style>