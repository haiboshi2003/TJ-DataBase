<template>
  <div class="body">
    <div class="headpic">
      <img class="logo" src="../../assets/image-2.png" />
      <div class="title">&nbsp;&nbsp;警务处理系统</div>
    </div>

    <div class="change-password-page">
      <div class="change-password-container">
        <p class="forget-text">忘记密码</p>
        <p class="setnew-text">重置密码</p>
        <form @submit.prevent="submitForm">
          <div class="input-container">
            <div>
              <input
                id="userName"
                type="text"
                v-model="PoliceNumber"
                autocomplete="off"
                @focus="handleFocus1"
                @blur="handleBlur1"
                required
              />
              <div class="Tip" :class="{ TipA: isFocused1 }">
                请填写您需要找回的警号
              </div>
            </div>

            <p v-if="!isValidStudentNumber" class="error-message">
              警号不满足格式
            </p>
            <p v-else>&nbsp;</p>
          </div>
          <div class="input-container">
            <div>
              <input
                id="idNumber"
                type="text"
                v-model="idNumber"
                autocomplete="off"
                required
                @focus="handleFocus2"
                @blur="handleBlur2"
              />
              <div class="Tip" :class="{ TipA: isFocused2 }">证件号码</div>
            </div>
            <p v-if="!isValidIdNumber" class="error-message">
              身份证号不满足格式
            </p>
            <p v-else>&nbsp;</p>
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
            </div>
            <div class="Tip" :class="{ TipA: isFocused3 }">输入验证码</div>
            <div class="code-image" @click="refreshCode">
              <!-- 验证码组件 -->
              <SIdentify :identifyCode="identifyCode"></SIdentify>
            </div>
          </div>

          <button type="submit" class="confirm-button">提交</button>
        </form>
        <p v-if="error" class="error-message">{{ error }}</p>
        <p v-else>&nbsp;</p>
      </div>
    </div>
  </div>
</template>

<script>
import axios from "../../api/request";

import SIdentify from "./Identify.vue";

export default {
  components: {
    SIdentify,
  },
  data() {
    return {
      PoliceNumber: "",
      idNumber: "",
      isValidStudentNumber: true,
      isValidIdNumber: true,
      error: "",
      //验证码
      formLogin: "",
      identifyCodes: "1234567890abcdefjhijklinopqrsduvwxyz", //随机串内容
      identifyCode: "",
      isFocused1: false,
      isFocused2: false,
      isFocused3: false,
    };
  },
  methods: {
    handleFocus1() {
      this.isFocused1 = true;
      console.log("点击了");
    },
    handleBlur1() {
      let val = document.getElementById("userName").value;
      if (!val) {
        this.isFocused1 = false;
      }
    },
    handleFocus2() {
      this.isFocused2 = true;
      console.log("点击了");
    },
    handleBlur2() {
      let val = document.getElementById("idNumber").value;
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

    //验证码
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
    //↑验证码
    submitForm() {
      // 清除之前的错误信息
      this.error = "";
      //验证码进行验证
      if (this.formLogin.toLowerCase() !== this.identifyCode.toLowerCase()) {
        this.error = "请填写正确验证码";
        setTimeout(() => {
          this.error = "";
        }, 1500);
        this.refreshCode();
        return;
      }
      // 格式验证
      this.isValidStudentNumber = /^\d{7}$/.test(this.PoliceNumber);
      this.isValidIdNumber = /^\d{18}$/.test(this.idNumber);

      // 验证通过才发送请求
      if (this.isValidStudentNumber && this.isValidIdNumber) {
        axios
          .post("/api/reset-password", {
            PoliceNumber: this.PoliceNumber,
            IdNumber: this.idNumber,
          })
          .then((response) => {
            this.refreshCode();
            // 处理响应
            if (response.data.success) {
              // 身份验证成功，存储token，并跳转到下一个页面
              sessionStorage.setItem("token", response.data.token);
              // 传递ID到另一个页面
              this.$router.push({
                path: "/ForPassword",
                query: { id: this.PoliceNumber }, // 传递警号参数
              });
            } else {
              // 身份验证失败，显示错误消息
              this.error = "身份不匹配，请重新输入";
              setTimeout(() => {
                this.error = "";
              }, 1500);
            }
          })
          .catch((error) => {
            // 处理错误
            this.refreshCode();
            console.error(error);
            this.error = "网络问题";
            setTimeout(() => {
              this.error = "";
            }, 1500);
          });
      }
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

.change-password-container {
  flex-direction: column;
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

.forget-text {
  color: #fff;
  font-size: 40px; /* 字号大小 */
  font-weight: bold; /* 字体加粗 */
  margin-top: 1px;
}
.setnew-text {
  color: #fff;
  margin-top: 1px;
  margin-bottom: 5px;
  font-size: 20px; /* 字号大小 */
  font-weight: bold; /* 字体加粗 */
}

#userName {
  margin-top: 50px;
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

#idNumber {
  margin-top: 50px;
  display: inline-block;
  width: 300px;
  height: 30px;
  color: #0ff;
  font-size: 20px;
  border: 0px transparent;
  border-bottom: 3px solid #fff;
  background-color: transparent;
}

#idNumber:focus {
  outline: none;
  border-bottom: 3px solid #0ff;
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

.Tip {
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
    font-size: 17px;
    color: rgb(12, 235, 179);
  }
}

.TipA {
  animation: user 0.3s linear normal forwards;
  animation-iteration-count: 1;
}

.input-container {
  flex-direction: column;
  margin-top: 20px;
  display: flex;
  position: relative;
  width: 400px;
  height: 150px;
  justify-content: center;
  align-items: center;
  font-size: 20px;
}

.confirm-button {
  background-color: #1890ff;
  color: white;
  padding: 10px 20px;
  border: none;
  border-radius: 10px;
  cursor: pointer;
  margin-left: 40%;
  font-weight: bold;
}

.confirm-button:hover {
  background-color: #40a9ff;
}

.error-message {
  color: red;
  font-weight: bold;
}

.code-container {
  margin-top: 10px;
  display: flex;
  position: relative;
  width: 400px;
  height: 80px;
  justify-content: center;
  align-items: center;
  font-size: 20px;
}

.code-image {
  flex: 1;
  cursor: pointer;
}
</style>
