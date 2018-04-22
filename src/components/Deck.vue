<template id="deck">
    <transition name="fade">
        <div v-if="deckModel.selectedDeck != null" v-show="deckModel.isModalVisible" class="modal-backdrop">
            <button type="button" class="close deck-close" @click="closeModal()">x</button>
            <div class="basic-modal full-screen-modal">
                <div class="deck-container">
                    <div class="card" v-for="card in deckModel.selectedDeck.cards" v-bind:key="card.id">
                        <div class="centered"> 
                            <h4 style="margin: 5px;">{{card.name}}</h4>
                        </div>
                        <div class="card-stats">
                            <div class="card-stat">
                                <p class="stat-label">Rarity:</p>
                                <p class="stat">{{card.rarity}}</p>
                            </div>
                            <div class="card-stat">
                                <p class="stat-label">Max Health:</p>
                                <p class="stat">{{card.maxHealth}}</p>
                            </div>
                            <div class="card-stat">
                                <p class="stat-label">Damage:</p>
                                <p class="stat">{{card.damage}}</p>
                            </div>
                            <div class="card-stat">
                                <p class="stat-label">Speed:</p>
                                <p class="stat">{{card.speed}}</p>
                            </div>
                            <div class="card-stat">
                                <p class="stat-label">Row:</p>
                                <p class="stat">{{card.row}}</p>
                            </div>
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
    </transition>
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
    .full-screen-modal {
        width: 90%;
        height: 98%;
    }

    .deck-container {
        display: flex;
        flex-direction: row;
        flex-wrap: wrap;
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

    .card-stats {
        font-size: .8em;
        border-top: #928e8e;
        border-top-style: double;
        padding-top: 3px;
    }

    .card-stat {
        display: grid;
        grid-template-columns: 50% 50%;
    }
        .card-stat > p {
            margin-bottom: 2px; 
        }

    .stat-label {
        text-align: left;
        padding-left: 5px;
    }

    .stat {
        text-align: center;
    }

    .card-description {
        font-size: .8em;
        margin-top: 4px;
        border-top: #928e8e;
        border-top-style: double;
        text-align: center;
        font-size: .7em;
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