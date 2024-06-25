<template>
  <el-header class="sub-header" @mousemove="handleMouseMove">
    <div>&nbsp;&nbsp;警员管理&nbsp;>&nbsp;警员考核管理</div>
  </el-header>
  <div class="container" @wheel.passive.stop>
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
          <b>搜索警员&nbsp;&nbsp;&nbsp;&nbsp;</b>
        </div>
        <!-- 搜索框 -->
        <input
          style="display: inline-block"
          v-model="policeNumber"
          placeholder="请输入正确的警号"
        />
        <span class="ssqpoptip">仅支持警号搜索!</span>
      </div>
      <!-- 确认按钮 -->
      <button class="cpbutton" @click="searchPolice"><span>搜索</span></button>
      <!-- 信息显示表格 -->
      <table class="custom-table" v-if="policeInfo">
        <thead>
          <tr>
            <th style="width: 100px">警号</th>
            <th style="width: 100px">姓名</th>
            <th style="width: 200px">当前职位</th>
          </tr>
        </thead>
        <tbody>
          <tr>
            <td class="cell-spacing">{{ policeInfo.police_number }}</td>
            <td class="cell-spacing">{{ policeInfo.police_name }}</td>
            <td class="cell-spacing">{{ policeInfo.position }}</td>
          </tr>
        </tbody>
      </table>

      <div v-if="policeInfo">
        <div style="margin-top: 30px; margin-left: 90px; font-size: 20px">
          <label style="color: #524c3e"
            ><b>申请修改至职位:&nbsp;&nbsp;&nbsp;&nbsp;</b></label
          >
          <select class="select" v-model="selectedPosition">
            <option value="学员">学员</option>
            <option value="警员">警员</option>
            <option value="警督">警督</option>
            <option value="警司">警司</option>
            <option value="警监">警监</option>
            <option value="总警监">总警监</option>
          </select>
        </div>

        <div style="margin-top: 20px; margin-left: 90px; font-size: 20px">
          <label style="top: 5px; color: #524c3e"
            ><b>请输入修改原因:&nbsp;&nbsp;&nbsp;&nbsp;</b></label
          >
          <el-input class="reasoninput" type="textarea" v-model="reason" />
        </div>
        <button
          class="cpbutton"
          style="line-height: 40px; width: 200px"
          @click="updatePosition"
        >
          <span>确认修改</span>
        </button>

        <!--提示窗口-->
        <el-dialog
          title="申请成功"
          v-model="updateSuccess"
          class="success-message"
        >
          修改成功
          <el-button
            style="display: block; margin-top: 20px"
            @click="handleClose"
            >确认</el-button
          >
        </el-dialog>
        <el-dialog
          title="申请失败"
          v-model="updateFailed"
          class="error-message"
        >
          申请失败!
          <el-button
            style="display: block; margin-top: 20px"
            @click="handleClose"
            >确认</el-button
          >
        </el-dialog>
        <el-dialog title="提示" v-model="policeNotFound" class="error-message">
          未找到警员信息!
          <el-button
            style="display: block; margin-top: 20px"
            @click="handleClose"
            >确认</el-button
          >
        </el-dialog>
      </div>
    </div>
  </div>
</template>

<script>
import axios from "../../api/request";

export default {
  data() {
    return {
      policeNumber: "",
      policeInfo: null,
      selectedPosition: "",
      reason: "",
      updateSuccess: false,
      updateFailed: false,
      policeNotFound: false,
    };
  },
  methods: {
    searchPolice() {
      this.policeInfo = axios
        .post("/api/search", { temp: this.policeNumber })
        .then((response) => {
          console.log(response.data);
          if (response.data == "未找到警员信息") {
            this.policeNotFound = true;
          } else {
            this.policeInfo = response.data;
          }

          //   this.updateSuccess = false
          //   this.updateFailed = false
        })
        .catch((error) => {
          console.error(error);
          this.policeInfo = null;
        });
    },
    async updatePosition() {
      // 模拟更新职位
      const isSuccess = await this.updatePositionInDatabase(
        this.policeInfo,
        this.selectedPosition,
        this.reason
      );
      if (isSuccess) {
        this.updateSuccess = true;
        this.updateFailed = false;
        this.searchPolice();
      } else {
        this.updateSuccess = false;
        this.updateFailed = true;
      }
    },
    updatePositionInDatabase(policeInfo, newPosition, reason) {
      return new Promise((resolve, reject) => {
        axios
          .post("/api/position", {
            police_number: policeInfo.police_number,
            police_name: policeInfo.police_name,
            position: newPosition,
            ID_number: policeInfo.ID_number,
            phone_number: policeInfo.phone_number,
            gender: policeInfo.gender,
          })
          .then((response) => {
            this.policeInfo = response.data;
            resolve(true);
          })
          .catch((error) => {
            console.error(error);
            resolve(false);
          });
      });
    },
    handleMouseMove(event) {
      const x = event.pageX - event.target.offsetLeft;
      const y = event.pageY - event.target.offsetTop;

      event.target.style.setProperty("--x", `${x}px`);
      event.target.style.setProperty("--y", `${y}px`);
    },
    handleClose() {
      this.updateSuccess = false;
      this.updateFailed = false;
      this.policeNotFound = false;
      location.reload();
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

.select {
  width: 200px;
  height: 30px;
  border: 1px solid #0051ff;
  box-shadow: #0051ff3a 0px 4px 5px;
  margin-left: 0px;
}
.reasoninput {
  width: 200px;
  border: 1px solid #0051ff;
  box-shadow: #0051ff3a 0px 4px 4px;
}
</style>
