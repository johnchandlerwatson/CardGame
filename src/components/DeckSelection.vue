<template id="decks">
  <div class="container">
      <h1>Select your deck!</h1>
      <div v-if="model != null" class="decks-container">
          <div class="deck" v-on:click="openDeck(deck)" v-for="deck in model.decks" v-bind:key="deck.id">
            <p>{{deck.name}}</p>
          </div>
      </div>
      <deck v-bind:deckModel="deckModel" v-on:goToArena="goToArena()"></deck>
  </div>
</template>

<script>
  import deck from './Deck.vue'

  export default {
    name: 'decks',
    components: { deck },
    data () {
      return {
        model: null,
        deckModel: {
          selectedDeck: null,
          isModalVisible: false
        }
      }
    },
    created () {
      this.$http
          .get('/api/Selection/')
          .then((res) => {
            console.log(res)
            this.model = res.body
          })
          .catch((ex) => console.log(ex))
    },
    methods: {
      openDeck: function (deck) {
        this.deckModel = {
          selectedDeck: deck,
          isModalVisible: true
        }
      },
      goToArena: function () {
        this.$emit('input', 'game')
      }
    }
  }
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
    .decks-container {
        display: flex;
        flex-direction: row;
        margin-top: 50px;
    }

    .deck {
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
    }
</style>