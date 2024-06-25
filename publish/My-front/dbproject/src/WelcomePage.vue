<template>
  <div class="body">
    <el-header
      style="height: 70px; padding: 0; text-align: justify; hyphens: auto"
    >
      <div class="headpic">
        <div class="header1">
          <img class="logo" src="../src/assets/image-2.png" />
          <div class="title">&nbsp;&nbsp;警务处理系统</div>
          <!-- <div class="footer1">返回首页</div> -->
        </div>
      </div>
    </el-header>

    <div class="main">
      <span></span>
      <span></span>
      <div id="clock">
        <p class="date">{{ date }}</p>
        <p class="time">{{ time }}</p>
        <p class="welcome-text">&nbsp;欢迎进入警务处理系统&nbsp;</p>
        <button class="loginbutton" @click="login">登录</button>
      </div>
    </div>
  </div>
</template>

<script>
var week = ["SUN", "MON", "TUE", "WED", "THU", "FRI", "SAT"];
export default {
  data() {
    return {
      time: "",
      date: "",
    };
  },
  mounted() {
    this.updateTime();
    this.timerID = setInterval(this.updateTime, 1000);
  },
  beforeUnmount() {
    clearInterval(this.timerID);
  },
  methods: {
    login() {
      this.$router.push("/login");
    },
    updateTime() {
      var cd = new Date();
      this.time =
        this.zeroPadding(cd.getHours(), 2) +
        ":" +
        this.zeroPadding(cd.getMinutes(), 2) +
        ":" +
        this.zeroPadding(cd.getSeconds(), 2);
      this.date =
        this.zeroPadding(cd.getFullYear(), 4) +
        "-" +
        this.zeroPadding(cd.getMonth() + 1, 2) +
        "-" +
        this.zeroPadding(cd.getDate(), 2) +
        " " +
        week[cd.getDay()];
    },
    zeroPadding(num, digit) {
      var zero = "";
      for (var i = 0; i < digit; i++) {
        zero += "0";
      }
      return (zero + num).slice(-digit);
    },
  },
};
</script>

<style lang="postcss" scoped>
.body {
  display: flex;
  height: 110vh;
  width: 100vw;
  overflow: hidden;
  flex-direction: column;
  background-image: url("assets/hellopolice.jpg");
  background-attachment: fixed;
  background-position: center;
  background-repeat: no-repeat;
  background-size: cover;
}

.main {
  overflow: auto;
  border-radius: 4px;
  background-color: rgba(84, 141, 240, 0.1);
  width: 90%;
  height: 75%;
  margin-top: 10vh;
  margin-left: 5vw;
  backdrop-filter: blur(6px);
  text-shadow: 0 1px 1px hsla(0, 0%, 100%, 0.3);
  box-shadow: 0 0 0 1px hsla(0, 0%, 100%, 0.3) inset,
    0 0.3em 1em rgba(0, 0, 0, 0.12);
  box-sizing: border-box;
  border: 1px solid #3d55f0;
}

.main:before {
  content: "";
  position: absolute;
  top: 0;
  left: -100%;
  width: 100%;
  height: 100%;
  background: rgba(255, 255, 255, 0.1);
  transition: 0.5s;
  pointer-events: none;
}

.main:hover:before {
  left: -50%;
  transform: skewX(-5deg);
}

.main span {
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  display: block;
  box-sizing: border-box;
  pointer-events: none;
}

.main span:nth-child(1) {
  transform: rotate(0deg);
}

.main span:nth-child(2) {
  transform: rotate(180deg);
}

.main span:before {
  content: "";
  position: absolute;
  width: 100%;
  height: 4px;
  background: #50dfdb;
  animation: animate 4s linear infinite;
}

@keyframes animate {
  0% {
    transform: scaleX(0);
    transform-origin: left;
  }
  50% {
    transform: scaleX(1);
    transform-origin: left;
  }
  50.1% {
    transform: scaleX(1);
    transform-origin: right;
  }
  100% {
    transform: scaleX(0);
    transform-origin: right;
  }
}

#clock {
  font-family: "Share Tech Mono", monospace;
  text-align: center;
  align-items: center;
  color: #ffffff;
  line-height: 1;
  text-shadow: 0 0 20px rgba(10, 175, 230, 1), 0 0 20px rgba(10, 175, 230, 0);
  font-weight: bold;
  .time {
    letter-spacing: 0.05em;
    font-size: 5em;
  }
  .date {
    margin-top: 5%;
    letter-spacing: 0.1em;
    font-size: 3em;
  }
  .welcome-text {
    letter-spacing: 0.1em;
    text-align: center;
    font-size: 5em; /* 字号大小 */
  }
  .loginbutton {
    letter-spacing: 5px;
    text-align: center;
    background-color: #1890ff;
    padding: 0.2em 0.4em;
    color: #fff;
    border: none;
    border-radius: 5px;
    cursor: pointer;
    font-size: 3em;
    font-weight: bold;
  }
}

.headpic {
  background: #ffffff;
  width: 100%;
  position: relative;
  overflow: hidden;
}

.header1 {
  background: #1f2cdf;
  background-image: url("assets/hdtest.jpg"); /* 替换为你的背景图路径 */
  background-size: contain;
  background-position: right top; /* 背景图靠左上角 */
  background-repeat: no-repeat;
  width: 100%;
  height: 70px;
  position: relative;
  left: 0;
  display: flex;
  align-items: center;
  padding-left: 20px;
}

.title {
  color: #ffffff;
  text-align: left;
  font: 400 36px "Inter", sans-serif;
  display: inline-block;
}
</style>
