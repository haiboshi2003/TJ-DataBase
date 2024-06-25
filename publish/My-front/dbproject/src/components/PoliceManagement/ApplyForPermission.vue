<template>
  <el-header class="sub-header" @mousemove="handleMouseMove">
    <div>
      &nbsp;&nbsp;警员管理&nbsp;>&nbsp;警员权限修改&nbsp;>&nbsp;申请修改权限
    </div>
  </el-header>
  <div class="container">
    <div class="ssqtitletest"><span>提&nbsp;交&nbsp;申&nbsp;请</span></div>
    <div class="content">
      <div style="margin-top: 40px; margin-left: 25%; font-size: 20px">
        <div style="display: inline-block; color: #524c3e">
          <b>警员警号:</b>
        </div>
        <div style="margin-left: 120px; display: inline-block; color: #4853ea">
          {{ parameter1 }}
        </div>
      </div>
      <div style="margin-top: 30px; margin-left: 25%; font-size: 20px">
        <div style="display: inline-block; color: #524c3e">
          <b>警员姓名:</b>
        </div>
        <div style="margin-left: 120px; display: inline-block; color: #4853ea">
          {{ parameter2 }}
        </div>
      </div>
      <div style="margin-top: 30px; margin-left: 25%; font-size: 20px">
        <label for="permission-select" style="color: #524c3e"
          ><b>申请修改至等级:</b></label
        >
        <select class="select" id="permission-select" v-model="selectedLevel">
          <option value="0">0</option>
          <option value="1">1</option>
          <option value="2">2</option>
          <option value="3">3</option>
          <option value="4">4</option>
          <option value="5">5</option>
        </select>
      </div>

      <div style="margin-top: 40px; margin-left: 25%; font-size: 20px">
        <label for="reason-input" style="top: 5px; color: #524c3e"
          ><b>修改理由:</b></label
        >
        <el-input
          class="reasoninput"
          type="textarea"
          id="reason-input"
          v-model="reason"
        />
      </div>
      <el-button
        class="cpbutton"
        style="line-height: 40px"
        @click="submitPermission"
        :disabled="disablesubmit"
        ><span>提交申请</span></el-button
      >
      <button class="cpbutton" style="margin-top: 20px" @click="returntoCP">
        <span>返回上一级</span>
      </button>
      <el-dialog
        title="提示"
        v-model="submittedSucessfully"
        @close="handleClose"
        >{{ this.returnmessage }}
        <el-button
          style="margin-top: 20px; right: 20px; display: block"
          @click="returntoCP"
          ><span>返回</span></el-button
        >
      </el-dialog>
    </div>
  </div>
</template>

<script>
import axios from "../../api/request";

export default {
  data() {
    return {
      parameter1: localStorage.getItem("CPid"),
      parameter2: localStorage.getItem("CPname"),
      my_number: localStorage.getItem("CPmynumber"),
      selectedLevel: "0", // 默认选择的权限级别为0
      reason: "", // 申请修改权限的理由
      disablesubmit: true,
      status: "待处理",
      submittedSucessfully: false,
      returnmessage: "",
    };
  },
  watch: {
    reason() {
      // 监听 reason 的变化，如果为空则禁用提交按钮，否则启用
      this.disablesubmit = this.reason.trim() === "";
    },
  },
  methods: {
    submitPermission() {
      // 在这里处理提交权限的逻辑
      axios
        .post("/api/permit", {
          h_number: this.my_number,
          s_number: this.parameter1,
          F_level: "0",
          L_level: this.selectedLevel,
          status: this.status,
          reason: this.reason,
        })
        .then((response) => {
          console.log(response);
          this.searchResult = response.data;
          this.returnmessage = response.data;
          this.submittedSucessfully = true;
        });
    },
    handleMouseMove(event) {
      const x = event.pageX - event.target.offsetLeft;
      const y = event.pageY - event.target.offsetTop;

      event.target.style.setProperty("--x", `${x}px`);
      event.target.style.setProperty("--y", `${y}px`);
    },
    returntoCP() {
      if (this.returnmessage == "申请提交成功") {
        this.$router.push("/mainMenu/ChangePermission");
      } else {
        this.$router.push("/mainMenu/ChangePermission");
        this.submittedSucessfully = false;
      }
    },
    handleClose() {
      this.submittedSucessfully = false;
    },
  },
};
</script>

<style scoped>
.ssqtitletest {
  margin: 10px auto;
  min-width: 800px;
  height: 40px;
  padding: 0 30px;
  line-height: 60px;
  text-align: center;
  position: relative;
  appearance: none;
  background: #4b55e0;
  box-shadow: inset -500px 0px 100px 0px #1d269b;
  border: none;
  color: white;
  font-size: 25px;
  outline: none;
  overflow: hidden;
  border-radius: 0px;
}
.ssqtitletest span {
  position: relative;
  top: -20%;
}
.sub-header {
  overflow: hidden;
  display: flex;
  position: absolute;
  top: 70px;
  left: 199px;
  width: calc(100% - 199px);
  height: 7vh;
  min-height: 40px;
  align-items: center; /* 文字竖直方向居中对齐 */
  background-color: #1f2cdf;
  box-shadow: inset -500px 0px 200px 0px rgba(4, 0, 113, 0.856);
  color: #ffffff;
  font-size: 28px;
}
.sub-header::before {
  --size: 0;
  content: "";
  position: absolute;
  left: var(--x);
  top: var(--y);
  width: var(--size);
  height: var(--size);
  background: radial-gradient(circle closest-side, #5a65ff, transparent);
  transform: translate(-50%, -50%);
  transition: width 0.2s ease, height 0.2s ease;
}
.sub-header:hover::before {
  --size: 400px;
}
.container {
  margin-top: 10vh;
  min-width: 700px;
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
}
.content {
  width: 50%;
  min-width: 800px;
  height: 850px;
  box-shadow: 0px 0px 10px 2px rgba(123, 103, 75, 0.427);
  background-color: rgba(255, 255, 255, 0.616);
  margin-bottom: 30px;
  display: flex;
  flex-direction: column;
  justify-content: flex-start;
  margin-top: 16px;
  padding: 16px;

  border-radius: 5px;
  box-shadow: #9a9a9a 0px 0px 6px;
  box-shadow: #777777 0px 0px 3px;
  border-top: #0051ff 3px solid;
  border-top: solid 3px transparent;
}
.select {
  width: 200px;
  height: 30px;
  border: 1px solid #0051ff;
  box-shadow: #0051ff3a 0px 4px 5px;
  margin-left: 60px;
}
.reasoninput {
  max-height: 400px;
  overflow: auto;
  margin-left: 120px;
  width: 200px;
  border: 1px solid #0051ff;
  box-shadow: #0051ff3a 0px 4px 4px;
}

.cpbutton {
  margin-top: 60px;
  margin-left: 55%;
  bottom: -6px;
  width: 120px;
  height: 40px;
  padding: 0 10px;
  line-height: 55px;
  text-align: center;
  position: relative;
  appearance: none;
  background: #4a54df;
  border: none;
  color: white;
  font-size: 20px;
  cursor: pointer;
  outline: none;
  overflow: hidden;
  border-radius: 100px;
  display: block;
}

.cpbutton span {
  position: relative;
  top: -20%;
}

.cpbutton::before {
  --size: 0;
  content: "";
  position: absolute;
  left: var(--x);
  top: var(--y);
  width: var(--size);
  height: var(--size);
  background: radial-gradient(circle closest-side, #d7caaa, transparent);
  transform: translate(-50%, -50%);
  transition: width 0.2s ease, height 0.2s ease;
}
.disabled-button {
  background: #e8e9f1;
}

.cpbutton:hover::before {
  --size: 400px;
}
</style>
