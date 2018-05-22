<template>
    <div v-if="model.Game != null" id="ally-cards" :class="handSectionClasses()">
        <div @click="selectedCard(ally.Name)" @mouseover="onHover(ally.Row)" @mouseleave="offHover()" class="card ally-card" :class="ally.Rarity.toLowerCase()" v-for="ally in model.Game.User1.Hand" :key="ally.Id">
            <span>{{ally.Name}}</span><br>
            <span>Health: {{ally.Health}}</span><br>
            <span>Damage: {{ally.Damage}}</span><br>
            <span>Rarity: {{ally.Rarity}}</span><br>
            <span>Side: {{ally.Row}}</span><br>
        </div>
        <timer ref="timer" v-if="!isBotGame()" v-on:forceTurn="selectedCard(firstCard())"></timer>

        <div v-if="disabled" class="disabled-overlay" id="disable-section">
            <h3>Waiting for oponent's move..</h3>
        </div>
    </div>
</template>

<script>
  import timer from './Timer.vue'
  export default {
    name: 'handcards',
    props: ['model', 'vertical', 'helloModel', 'disabled'],
    components: { timer },
    data () {
      return {
        hoverState: {
          frontHover: false,
          backHover: false
        }
      }
    },
    methods: {
      selectedCard: function (selection) {
        this.hoverState.backHover = false
        this.hoverState.frontHover = false
        this.$emit('selectedCard', selection)
      },
      firstCard: function () {
        return this.model.Game.User1.Hand[0].Name
      },
      isBotGame: function () {
        return this.helloModel.gameId === null
      },
      onHover: function (row) {
        if (row === 'Front') {
          this.hoverState.frontHover = true
        } else {
          this.hoverState.backHover = true
        }
        this.$emit('hoverStateChange', this.hoverState)
      },
      offHover: function () {
        this.hoverState.frontHover = false
        this.hoverState.backHover = false
        this.$emit('hoverStateChange', this.hoverState)
      },
      handSectionClasses () {
        var orientationClass = this.vertical ? 'hand-section-vertical' : 'hand-section-horizontal'
        if (!this.disabled) return orientationClass
        return orientationClass + ' disabled'
      },
      resetTimer () {
        this.$refs.timer.resetTimer()
      }
    }
  }
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
    .hand-section-vertical {
        display: flex; 
        flex-direction: column;
        background-color: rgb(156, 156, 162);
        position: relative;
        width: 100%;
    }

    .disabled-overlay {
        opacity: 0.7;
        background: #626263;
        z-index: 4;
        position: absolute;
        bottom: 0;
        height: 100%;
        width: 100%;
        color: white;
        text-align: center;
        font-size: 2em;
        padding-top: 20px;
    }

    .disabled {
        pointer-events: none;
    }

    .ally-card {
        cursor: pointer;
        font-size: .8em;
        flex-grow: 1;
        flex-basis: 0;
        max-width: 120px;
        min-width: 90px;
    }

        .ally-card:hover {
            border: #c3dbdb solid 4px;
        }
</style>