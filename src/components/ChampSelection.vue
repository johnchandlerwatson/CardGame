<template id="champs">
  <div class="container">
      <h2>Select your champion!</h2>
      <div v-if="model != null" class="champs-container">
          <div class="champion" v-on:click="goToArena(champ.name)" v-for="champ in model.champs" v-bind:key="champ.id">
            <img class="champ-img" :src="getImage(champ.champImage)">
            <p>{{champ.name}}</p>
          </div>
      </div>
  </div>
</template>

<script>
  export default {
    name: 'champs',
    props: ['helloModel'],
    data () {
      return {
        model: null
      }
    },
    created () {
      this.$http
          .get('/api/Selection/Champs')
          .then((res) => {
            this.model = res.body
          })
          .catch((ex) => console.log(ex))
    },
    methods: {
      goToArena: function (champName) {
        this.$emit('input', { componentName: 'gameLobby', deckName: this.helloModel.deckName, champName: champName, username: this.helloModel.username })
      },
      getImage: function (picName) {
        var images = require.context('../assets/', false, /\.png$/)
        return images('./' + picName + '.png')
      }
    }
  }
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
    .champs-container {
        display: flex;
        flex-direction: row;
        margin-top: 50px;
    }

    .champ-img {
        margin-left: 20px;
        width: 80px;
    }

    .champion {
        padding: 10px;
        background-color: rgb(105, 100, 100);
        border-radius: 5px;
        height: 200px;
        width: 150px;
        display: grid;
        align-items: center;
        align-content: center;
        text-align: center;
        color: white;
        font-size: 1.5em;
        cursor: pointer;
        margin-left: 20px;
    }
</style>