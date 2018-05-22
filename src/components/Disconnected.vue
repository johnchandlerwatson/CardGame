<template>
    <div class="disconnected centered">
        <h3 style="padding: 20px;">The other player has disconnected :(</h3>
        <button class="btn btn-success lobby-button" @click="returnToLobby()">Return to Lobby</button>
    </div>
</template>

<script>
  export default {
    name: 'disconnected',
    methods: {
      returnToLobby: function () {
        this.$emit('goToLobby')
      }
    }
  }
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
    .disconnected {
        position: absolute;
        top: calc(30%);
        left: calc(50%);
        margin-left: -40%;
        width: 75%;
        background: white;
        border-radius: 20px;
        box-shadow: black 6px 6px 20px 2px;
    }

    .lobby-button {
        margin: auto;
        margin-bottom: 10px;
    }
</style>