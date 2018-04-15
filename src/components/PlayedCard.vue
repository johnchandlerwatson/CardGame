<template>
    <div class="played-card" :class="{'attack-ally': shouldAttackAlly, 'attack-enemy': shouldAttackEnemy}">
        <span class="centered">{{card.Name}}</span><br>
        <div class="stats">
            <p>HLTH: {{card.Health}}</p>
            <p style="text-align: right;">DMG: {{card.Damage}}</p>
        </div>
    </div>
</template>

<script>
  export default {
    name: 'playedCard',
    props: ['card', 'isEnemy'],
    data: function () {
      return {
        shouldAttackAlly: this.isEnemy,
        shouldAttackEnemy: !this.isEnemy
      }
    },
    created: function () { //removing class so we add back and animate again
      var self = this
      setTimeout(function () {
        self.shouldAttackAlly = false
        self.shouldAttackEnemy = false
      }, 1500)
    }
  }
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
    .played-card {
        background: linear-gradient(to right, #6b5d5d , #504d4d);
        color: #f3eeee;
        margin: 10px;
        border-radius: 4px;
        padding: 10px;
        width: 125px;
        font-size: .8em;
    }

    .stats {
        display: grid;
        grid-template-columns: 50% 50%;
    }

    .stats > p {
        margin: 0;
    }

    .attack-enemy { 
        animation: attack-enemy 0.4s both;
        position: relative;
        animation-delay: .5s;
    }

    .attack-ally { 
        animation: attack-ally 0.4s both;
        position: relative;
        animation-delay: .5s;
    }

    @keyframes attack-enemy {
        0%   {left:0px; top:0px;}
        50%  {left:0px; top:-50px;}
        75%  {left:0px; top:-75px;}
        100% {left:0px; top:0px;}
    }

    @keyframes attack-ally {
        0%   {left:0px; top:0px;}
        50%  {left:0px; top:50px;}
        75%  {left:0px; top:75px;}
        100% {left:0px; top:0px;}
    }
</style>