<template>
  <div class="body">
    <div class="headpic">
      <img class="logo" src="../../assets/image-2.png" />
      <div class="title">&nbsp;&nbsp;警务处理系统</div>
    </div>

    <div class="login-page">
      <div class="login-container">
        <section>
          <form @submit.prevent="submitForm">
            <p class="login-text">登&nbsp;&nbsp;&nbsp;录</p>
            <div class="put">
              <div>
                <input
                  id="userName"
                  type="text"
                  v-model="username"
                  autocomplete="off"
                  @focus="handleFocus1"
                  @blur="handleBlur1"
                  required
                />
                <div class="userNameTip" :class="{ userNameTipA: isFocused }">
                  账号
                </div>
              </div>
            </div>
            <div class="put">
              <div>
                <input
                  id="password"
                  type="password"
                  autocomplete="off"
                  v-model="password"
                  required
                  @focus="handleFocus2"
                  @blur="handleBlur2"
                />
                <div class="userNameTip" :class="{ userNameTipA: isFocused2 }">
                  密码
                </div>
              </div>
            </div>
            <div class="code-container">
              <div>
                <input
                  id="two-code-text"
                  v-model="formLogin"
                  autocomplete="off"
                  @focus="handleFocus3"
                  @blur="handleBlur3"
                />
                <div class="userNameTip" :class="{ userNameTipA: isFocused3 }">
                  验证码
                </div>
              </div>

              <div class="code-image" @click="refreshCode">
                <!-- 验证码组件 -->
                <SIdentify :identifyCode="identifyCode"></SIdentify>
              </div>
            </div>
            <div class="bottombox">
              <button class="confirmlogin" type="submit">确认登录</button>
              <div class="password-link">
                <a class="password-link-text" @click="pagechange">修改密码</a>
              </div>
            </div>
            <div class="code-feedbackbox">
              <p v-if="error" class="error-message">{{ error }}</p>
              <p v-else>&nbsp;</p>
            </div>
          </form>
        </section>
      </div>
    </div>
  </div>
</template>

<script>
import axios from "../../api/request";
import SIdentify from "./Identify.vue";
import jwt_decode from "jwt-decode";

export default {
  components: {
    SIdentify,
  },
  data() {
    return {
      username: "",
      password: "",
      error: "",
      //验证码
      formLogin: "",
      identifyCodes: "1234567890abcdefjhijklinopqrsduvwxyz", //随机串内容
      identifyCode: "",
      //鼠标点击
      isFocused: false,
      isFocused2: false,
      isFocused3: false,
    };
  },
  methods: {
    handleFocus1() {
      this.isFocused = true;
      console.log("点击了");
    },
    handleBlur1() {
      let val = document.getElementById("userName").value;
      if (!val) {
        this.isFocused = false;
      }
    },
    pagechange() {
      this.$router.push("/changepassword");
    },
    handleFocus2() {
      this.isFocused2 = true;
      console.log("点击了");
    },
    handleBlur2() {
      let val = document.getElementById("password").value;
      if (!val) {
        this.isFocused2 = false;
      }
    },
    handleFocus3() {
      this.isFocused3 = true;
      console.log("点击了");
    },
    handleBlur3() {
      let val = document.getElementById("two-code-text").value;
      if (!val) {
        this.isFocused3 = false;
      }
    },

    refreshCode() {
      this.identifyCode = "";
      this.makeCode(this.identifyCodes, 4);
    },
    makeCode(o, l) {
      for (let i = 0; i < l; i++) {
        this.identifyCode +=
          this.identifyCodes[this.randomNum(0, this.identifyCodes.length)];
      }
    },
    randomNum(min, max) {
      return Math.floor(Math.random() * (max - min) + min);
    },
    submitForm() {
      // 清除之前的错误信息 用于输入错误的时候
      //this.error = "";

      // 在这里执行登录逻辑，可以调用后端API进行验证
      // 使用 this.username 和 this.password 获取账号和密码
      const loginData = {
        username: this.username,
        password: this.password,
      };
      if (this.formLogin.toLowerCase() !== this.identifyCode.toLowerCase()) {
        this.error = "请填写正确验证码";
        setTimeout(() => {
          this.error = "";
        }, 1500);
        this.refreshCode();
        return;
      }

      axios
        .post("/api/login", loginData)
        .then((response) => {
          if (response.data.success) {
            // 登录成功
            this.refreshCode();
            // 获取JWT令牌
            const token = response.data.token;

            localStorage.clear();

            // 将JWT令牌存储在浏览器的本地存储中
            localStorage.setItem("token", token);
            // this.$router.push({
            //   name: "success",
            //   params: {
            //     username: this.username,
            //   },
            // });
            const police_number =
              jwt_decode(token)[
                "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"
              ];
            const authority =
              jwt_decode(token)[
                "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
              ];
            localStorage.setItem("policeNumber", police_number);
            localStorage.setItem("authority", authority);
            localStorage.setItem("loginErr", "false");
            this.$router.push("/mainMenu");
          } else {
            // 登录失败，显示错误提示
            this.refreshCode();
            this.error = "账号或密码错误，请重新输入";
            setTimeout(() => {
              this.error = "";
            }, 1500);
          }
        })
        .catch((error) => {
          // 处理错误
          console.error(error);
          this.refreshCode();
          this.error = "登录失败，请稍后重试";
          setTimeout(() => {
            this.error = "";
          }, 1500);
        });
    },
  },
  mounted() {
    // 初始化验证码
    this.identifyCode = "";
    this.makeCode(this.identifyCodes, 4);
  },
};
</script>

<style lang="postcss" scoped>
.password-link-text {
  text-decoration: underline;
}
.body {
  background-image: url("../../assets/hellopolice.jpg");
  background-attachment: fixed;
  background-position: center;
  background-repeat: no-repeat;
  background-size: cover;
  height: 100vh;
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

.login-container {
  display: flex;
  margin: 0 auto;
  margin-top: 50px;
  padding: 20px;
  border-radius: 4px;
  height: 70%;
  width: 30%;
  max-width: 35%;
  min-width: 400px;
  justify-content: center;
  align-items: center;
  border-radius: 0.3em;
  backdrop-filter: blur(2px);
  background-color: #2b2828ba;
  box-shadow: 0 0 0 10px hsla(0, 2%, 73%, 0.711) inset,
    0 0.3em 1em rgba(0, 0, 0, 0.12);
}

.login-text {
  color: #fff;
  text-align: center;
  font-size: 40px; /* 字号大小 */
  font-weight: bold; /* 字体加粗 */
}

#two-code-text {
  display: inline-block;
  width: 150px;
  height: 30px;
  color: #0ff;
  font-size: 20px;
  border: 0px transparent;
  border-bottom: 3px solid #fff;
  margin-left: 70px;
  background-color: transparent;
}

#two-code-text:focus {
  outline: none;
  border-bottom: 3px solid #0ff;
}

.put {
  display: flex;
  position: relative;
  width: 400px;
  height: 100px;
  justify-content: center;
  align-items: center;
  font-size: 20px;
}

#userName {
  display: inline-block;
  width: 300px;
  height: 30px;
  color: #0ff;
  font-size: 20px;
  border: 0px transparent;
  border-bottom: 3px solid #fff;
  background-color: transparent;
}

#userName:focus {
  outline: none;
  border-bottom: 3px solid #0ff;
}

#password {
  display: inline-block;
  width: 300px;
  height: 30px;
  color: #0ff;
  font-size: 20px;
  border: 0px transparent;
  border-bottom: 3px solid #fff;
  background-color: transparent;
}

#password:focus {
  outline: none;
  border-bottom: 3px solid #0ff;
}

.userNameTip {
  position: absolute;
  top: 0px;
  left: 20px;
  font-size: 20px;
  font-weight: bold;
  color: #fff;
}
@keyframes user {
  from {
    top: 0px;
    font-size: 20px;
  }
  to {
    top: -20px;
    font-size: 20px;
    color: rgb(166, 243, 166);
  }
}

.userNameTipA {
  animation: user 0.3s linear normal forwards;
  animation-iteration-count: 1;
}

button {
  padding: 10px 20px;
  background-color: #4caf60;
  color: #fff;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  font-size: 18px;
  font-weight: bold;
}

.error-message {
  color: red;
  margin-top: 20px;
  font-weight: bold;
}

.code-container {
  display: flex;
  position: relative;
  width: 400px;
  height: 100px;
  justify-content: center;
  align-items: center;
  font-size: 20px;
}

.code-feedbackbox {
  text-align: center;
  bottom: 0;
}

.code-image {
  flex: 1;
  cursor: pointer;
}

.bottombox {
  width: 100%;
  height: 50px;
}

.confirmlogin {
  float: right;
  margin-right: 80px;
}

.password-link {
  margin-left: 80px;
}
.password-link a {
  color: white;
}
</style>
