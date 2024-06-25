<template>
  <div class="body">
    <div class="headpic">
      <img class="logo" src="../../assets/image-2.png" />
      <div class="title">&nbsp;&nbsp;警务处理系统</div>
    </div>

    <div class="success-page-container">
      <p data-text="密码修改成功!">密码修改成功!</p>
      <p data-text="您的密码已成功修改">您的密码已成功修改</p>
      <button class="back-to-loginbtn" @click="goToLogin">返回登录</button>
    </div>
  </div>
</template>

<script>
export default {
  methods: {
    goToLogin() {
      this.$router.push("/login");
    }
  }
};
</script>

<style lang="postcss" scoped>
.success-page-container {
  overflow: auto;
  flex-direction: column;
  display: flex;
  margin: 0 auto;
  margin-top: 10vh;
  padding: 20px;
  border-radius: 4px;
  height: 60%;
  min-height: 500px;
  width: 50%;
  min-width: 400px;
  justify-content: center;
  align-items: center;
  border-radius: 0.3em;
  backdrop-filter: blur(2px);
  background-color: #2b2828ba;
  box-shadow: 0 0 0 10px hsla(0, 2%, 73%, 0.711) inset,
    0 0.3em 1em rgba(0, 0, 0, 0.12);
}
.body {
  background-image: url("../../assets/hellopolice.jpg");
  background-attachment: fixed;
  background-position: center;
  background-repeat: no-repeat;
  background-size: cover;
  height: 120vh;
  overflow: auto;
}

.headpic {
  background: #1f2cdf;
  background-image: url("../../assets/hdtest.jpg"); /* 替换为你的背景图路径 */
  background-size: contain;
  background-position: right top; /* 背景图靠左上角 */
  background-repeat: no-repeat;
  width: 100%;
  height: 70px;
  overflow: hidden;
  position: relative;
  left: 0;
  display: flex;
  align-items: center;
  padding-left: 20px;
  .logo {
    width: 70px;
    height: 70px;
    position: relative;
    top: 2px;
    left: 0px;
  }
  .title {
    color: #ffffff;
    text-align: left;
    font: 400 36px "Inter", sans-serif;
    display: inline-block;
  }
}

.back-to-loginbtn {
  margin-top: 10px;
  margin-bottom: 30px;
  background-color: #1890ff;
  padding: 10px 20px;
  color: #fff;
  border: none;
  border-radius: 5px;
  cursor: pointer;
  font-size: 30px;
  font-weight: bold;
  letter-spacing: 0.1em;
}

p {
  
  position: relative;
  margin-top: 0.5em;
  font-size: 4em;
  font-weight: bold;
  letter-spacing: 0.2em;
  display: inline-block;
  line-height: 1;
  white-space: nowrap;
  color: transparent;
  background-color: rgba(10, 175, 230, 1);
  background-clip: text;
  -webkit-background-clip: text;
}

p::after {
  content: attr(data-text);
  position: absolute;
  left: 0;
  top: 0;
  width: 100%;
  height: 100%;
  z-index: 5;
  background-image: linear-gradient(
    120deg,
    transparent 0%,
    transparent 6rem,
    white 11rem,
    transparent 11.15rem,
    transparent 15rem,
    rgba(255, 255, 255, 0.3) 20rem,
    transparent 25rem,
    transparent 27rem,
    rgba(255, 255, 255, 0.6) 32rem,
    white 33rem,
    rgba(255, 255, 255, 0.3) 33.15rem,
    transparent 38rem,
    transparent 40rem,
    rgba(255, 255, 255, 0.3) 45rem,
    transparent 50rem,
    transparent 100%
  );
  background-clip: text;
  -webkit-background-clip: text;
  background-size: 150% 100%;
  background-repeat: no-repeat;
  animation: shine 5s infinite linear;
}

@keyframes shine {
  0% {
    background-position: 50% 0;
  }
  100% {
    background-position: -190% 0;
  }
}
</style>
