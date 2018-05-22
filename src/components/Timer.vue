<template>
    <div class="centered timer">
        <p :class="{'normal': !isUrgent, 'urgent': isUrgent}">Round Ending: {{seconds}}</p>
    </div>
</template>

<script>
  export default {
    name: 'timer',
    data: function () {
      return {
        seconds: 30,
        isUrgent: false
      }
    },
    created: function () {
      this.startTimer()
    },
    methods: {
      startTimer: function () {
        var self = this
        var id = window.setInterval(() => {
          self.seconds = self.seconds - 1
          if (self.seconds === 0) {
            self.$emit('forceTurn')
            window.clearInterval(id)
            self.resetTimer()
            self.startTimer()
          } else if (self.seconds === 10) {
            self.isUrgent = true
          }
        }, 1000)
      },
      resetTimer: function resetTimer () {
        this.isUrgent = false
        this.seconds = 30
      }
    }
  }
</script>

<style scoped>
    .timer {
        margin-right: 10px;
        margin-left: auto;
    }

    .normal {
        color: black;
    }

    .urgent {
        color: #ab2020;
    }
</style>