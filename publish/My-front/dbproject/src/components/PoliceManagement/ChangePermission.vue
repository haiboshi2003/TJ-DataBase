<template>
  <el-header class="sub-header" @mousemove="handleMouseMove">
    <div>&nbsp;&nbsp;警员管理&nbsp;>&nbsp;警员权限修改</div>
  </el-header>

  <div class="container">
    <div class="content">
      <div style="margin-top: 10px; margin-left: 50px; font-size: 20px">
        <div
          style="
            margin-top: 40px;
            margin-left: 40px;
            display: inline-block;
            color: #524c3e;
          "
        >
          搜索警员&nbsp;&nbsp;&nbsp;&nbsp;
        </div>
        <!-- 搜索框 -->
        <input
          style="display: inline-block"
          v-model="searchKeyword"
          placeholder="请输入正确的警号"
        />
        <span class="ssqpoptip">仅支持警号搜索!</span>
      </div>
      <!-- 确认按钮 -->
      <button class="cpbutton" @mousemove="handleMouseMove" @click="search">
        <span>确认</span>
      </button>
      <!-- 信息显示表格 -->
      <table class="custom-table" v-if="searchResult">
        <thead>
          <tr>
            <th style="width: 100px">警号</th>
            <th style="width: 100px">姓名</th>
            <th style="width: 200px">身份证号码</th>
            <th style="width: 200px">电话号码</th>
          </tr>
        </thead>
        <tbody>
          <tr>
            <td class="cell-spacing">{{ searchResult.police_number }}</td>
            <td class="cell-spacing">{{ searchResult.police_name }}</td>
            <td class="cell-spacing">{{ searchResult.iD_number }}</td>
            <td class="cell-spacing">{{ searchResult.phone_number }}</td>
          </tr>
        </tbody>
      </table>

      <div class="center-align">
        <div class="middle-align">
          <button
            class="cpbutton"
            style="margin-left: 60px; width: 180px"
            @mousemove="handleMouseMove"
            @click="modifyPermissions"
            :disabled="disableButton"
          >
            <span>申请修改权限</span>
          </button>
        </div>
      </div>

      <!-- 错误提示模态框 -->
      <el-dialog
        title="错误提示"
        v-model="showErrorMessage"
        @close="handleClose"
        >{{ errorMessage }}</el-dialog
      >
    </div>
  </div>
</template>

<script>
import axios from "../../api/request";

export default {
  data() {
    return {
      searchKeyword: "",
      searchResult: {},
      selectedPermission: "read",
      showErrorMessage: false,
      errorMessage: "",
      disableButton: true,
    };
  },
  methods: {
    search() {
      this.searchResult = {};
      this.errorMessage = "";
      if (!this.searchKeyword) {
        this.errorMessage = "搜索关键字不能为空";
        this.showErrorMessage = true;
        return;
      }

      // 根据搜索关键字执行相应的逻辑，比如发送请求获取数据并更新 searchResult
      axios
        .post("/api/search", { temp: this.searchKeyword })
        .then((response) => {
          this.searchResult = response.data;
          this.disableButton = false;
        })
        .catch((error) => {
          console.error(error);
          this.searchResult = {};
          this.errorMessage = "搜索失败，请输入正确警号或稍后重试";
          this.showErrorMessage = true;
          this.disableButton = true;
        });
    },
    modifyPermissions() {
      localStorage.setItem("CPid", this.searchResult.police_number);
      localStorage.setItem("CPname", this.searchResult.police_name);
      localStorage.setItem("CPmynumber", "6982809");
      this.$router.push("/mainMenu/ApplyForPermission");
    },
    closeErrorMessage() {
      this.showErrorMessage = false;
    },
    handleMouseMove(event) {
      const x = event.pageX - event.target.offsetLeft;
      const y = event.pageY - event.target.offsetTop;

      event.target.style.setProperty("--x", `${x}px`);
      event.target.style.setProperty("--y", `${y}px`);
    },
    handleClose() {
      this.showErrorMessage = false;
    },
  },
};
</script>

<style scoped>
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
  min-width: 1000px;
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
.custom-table {
  border-collapse: collapse;
  margin-top: 60px;
  margin-left: 80px;
  width: 800px;
  position: relative;
  border: 1px solid #ccc;
  box-shadow: 0px 5px 5px 0px #dbd1bb90;
}

.custom-table th,
.custom-table td {
  border: 1px solid rgb(224, 224, 224);
  padding: 8px;
  height: 50px;
  text-align: center;
  position: relative;
}

.custom-table th {
  background-color: rgb(255, 255, 255);

  color: #94a6cd;
}
.custom-table td {
  background-color: rgb(255, 255, 255);
  color: #585e6d;
}

.cell-spacing {
  padding: 4px;
}

.cpbutton {
  margin-top: 15px;
  margin-left: 80px;
  bottom: -6px;
  width: 80px;
  height: 30px;
  padding: 0 10px;
  line-height: 40px;
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

.cpbutton:hover::before {
  --size: 400px;
}

input {
  display: inline-block;
  padding-left: 20px;
  height: 35px;
  width: 229px;
  outline: none;
  font-size: 14px;
  border: 1px solid #c9bb9e;
  border-radius: 2px;
  box-shadow: #b4a078 0px 0px 2px;
  transition: border-color 0.2s ease-in-out;
  transition: box-shadow 0.2s ease-in-out;
}
input:focus,
input:hover {
  border-color: #6777ef;
  box-shadow: #3d4cc1 0px 0px 2px;
}

input:not(:focus) + .ssqpoptip {
  transform: scale(0);
  animation: elastic-dec 0.25s;
}

input:focus + .ssqpoptip {
  transform: scale(1);
  animation: elastic-grow 0.45s;
}
.ssqpoptip {
  display: inline-block;
  width: 150px;
  font-size: 13px;
  color: #867656;
  padding: 0.6em;
  background: #fafafa;
  position: relative;
  margin-left: -3px;
  margin-top: 3px;
  border-radius: 2px;
  filter: drop-shadow(0 0 1px #c9bb9e);
  transform-origin: 15px -6px;
}
.ssqpoptip::before {
  content: "";
  position: absolute;
  top: 50%;
  left: -10px;
  border: 9px solid transparent;
  border-bottom-color: #fafafa;
  border-top-width: 0;
  padding: 3px;
}
@keyframes elastic-grow {
  from {
    transform: scale(0);
  }
  70% {
    transform: scale(1.1);
    animation-timing-function: cubic-bezier(0.1, 0.25, 0.1, 0.25);
  }
}
@keyframes elastic-dec {
  from {
    transform: scale(1);
  }
  to {
    transform: scale(0);
    animation-timing-function: cubic-bezier(0.25, 0.1, 0.25, 0.1);
  }
}

* {
  box-sizing: border-box;
}
</style>
