<template id="home">
  <div id="app" class="container">
    <img src="./../assets/logo.png">
    <img class="gif" src="./../assets/trippy.gif">
    <div class="hello">
      <h2>Welcome to</h2>
      <h1 style="font-size: 6em;">THE VOID</h1>
      <p>{{ msg }}</p>
      <div class="container">
        <div class="row">
          <div class="col-sm-12">
            <button v-on:click="showModal" class="btn btn-lg btn-success">Play!</button>
          </div>
        </div>
      </div>
      <div class="modal fade" id="play-modal" tabindex="-1" role="dialog">
        <div class="modal-dialog" role="document">
          <div class="modal-content">
            <div class="modal-header">
              <h2 class="modal-title">PLAY INFLUENCE!</h2>
            </div>
            <div class="modal-body">
              <form id="play-form">
                  <div class="form-field">
                      <label for="username">Username:</label>
                      <input id="username" type="text" name="username">
                  </div>
              </form>
            </div>
            <div class="modal-footer">
              <button type="button" v-on:click="goToSelection()" class="btn btn-primary">PLAY!</button>
              <button type="button" class="btn btn-secondary" data-dismiss="modal">Nah</button>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
  import $ from 'jquery'

  export default {
    name: 'home',
    data () {
      return {
        msg: ''
      }
    },
    // Send a request to /api/home
    created () {
      this.$http
          .get('/api/home')
          .then((res) => {
            this.msg = 'Here are the new decks: ' + res.body.decks.join(', ')
          })
          .catch((ex) => console.log(ex))
    },
    methods: {
      showModal: function (event) {
        $('#play-modal').modal('show')
      },
      goToSelection: function () {
        $('#play-modal').modal('hide')
        this.$emit('input', { componentName: 'decks' })
      }
    }
  }
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
  h1, h2 {
    font-weight: normal;
  }

  #app {
    font-family: 'Avenir', Helvetica, Arial, sans-serif;
    -webkit-font-smoothing: antialiased;
    -moz-osx-font-smoothing: grayscale;
    text-align: center;
    color: #2c3e50;
    margin-top: 60px;
  }

  .gif {
    position: fixed;
    height: 100%;
    width: 100%;
    top: 0;
    z-index: -1;
    left: 0;
    opacity: .15;
  }
</style>
