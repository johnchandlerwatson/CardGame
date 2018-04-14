<template id="deck">
    <div v-if="deckModel.selectedDeck != null" v-show="deckModel.isModalVisible" class="modal-backdrop">
        <button type="button" class="close deck-close" @click="closeModal()">x</button>
        <div class="deck-modal">
            <div class="deck-container">
                <div class="card" v-for="card in deckModel.selectedDeck.cards" v-bind:key="card.id">
                    <div class="card-header"> 
                        <p>{{card.name}}</p>
                    </div>
                    <div class="card-stats">
                        <p>Rarity: {{card.rarity}}</p>
                        <p>Max Health: {{card.maxHealth}}</p>
                        <p>Damage: {{card.damage}}</p>
                        <p>Speed: {{card.speed}}</p>
                        <p>Row: {{card.row}}</p>
                    </div>
                    <div class="card-description">
                        <p>{{card.description}}</p>
                    </div>
                </div>
            </div>
            <div class="button-section">
                <button class="btn btn-success" v-on:click="goToChamp(deckModel.selectedDeck.name)">Heck Yeah!</button>
                <button class="btn btn-secondary" v-on:click="closeModal()">Nah</button>
            </div>
        </div>
    </div>
</template>

<script>
  export default {
    name: 'deck',
    props: ['deckModel'],
    created () {
      document.addEventListener('keyup', this.closeOnEscape)
    },
    methods: {
      closeModal: function () {
        this.deckModel.isModalVisible = false
      },
      closeOnEscape: function (e) {
        if (e.keyCode === 27) {
          this.deckModel.isModalVisible = false
        }
      },
      goToChamp: function (deckName) {
        this.$emit('input', deckName)
        this.$emit('goToChamp')
      }
    }
  }
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
    .deck-container {
        display: flex;
        flex-direction: row;
        flex-wrap: wrap;
    }

    .deck-modal {
        box-shadow: 5px 5px 20px 0px;
        overflow-x: auto;
        position: fixed;
        z-index: 3050;
        overflow: hidden;
        background: white;
        width: 90%;
        height: 98%;
        border-radius: 10px;
        padding: 10px;
        display: grid;
        grid-template-rows: 95% 5%;
        transition: opacity .25s ease-in-out;
        -moz-transition: opacity .25s ease-in-out;
        -webkit-transition: opacity .25s ease-in-out;
    }

    .modal-backdrop {
        position: fixed;
        top: 0;
        bottom: 0;
        left: 0;
        right: 0;
        background-color: rgba(0, 0, 0, 0.3);
        display: flex;
        justify-content: center;
        align-items: center;
    }

    .card {
        background-color: rgb(196, 194, 194);
        border-radius: 5px;
        padding: 5px;
        height: 210px;
        width: 150px;
        margin-right: 10px;
        margin-bottom: 10px;
    }

    .card-header {
        font-weight: bold;
        font-size: 1.2em;
    }

        .card-header > p {
            margin: 0;
        }

    .card-stats {
        font-size: .8em;
        border-top: #928e8e;
        border-top-style: double;
        padding-top: 3px;
    }

        .card-stats > p {
            margin-bottom: 2px; 
        }

    .card-description {
        font-size: .8em;
        margin-top: 4px;
        border-top: #928e8e;
        border-top-style: double;
    }

    .button-section {
        display: flex;
        flex-direction: row;
        justify-content: flex-end;
    }

    .deck-close {
        right: 0;
        top: 0;
        position: fixed;
        width: 5%;
        font-size: 3.5em;
    }
</style>