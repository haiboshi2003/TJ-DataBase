<template>
  <el-header class="sub-header" @mousemove="handleMouseMove">
    <div>&nbsp;&nbsp;出勤管理&nbsp;>&nbsp;出勤出勤管理</div>
  </el-header>

  <el-tabs v-model="anv" type="border-card" style="margin-top: 10vh;">
    <!-- 查询栏 -->
    <el-tab-pane label="出勤记录" name="1">
      <div style="text-align: center;">
        <table style="display: inline-block;text-align: left;">
          <tr>
            <td>
              <el-text class="noteText" type="primary">出勤警员编号:</el-text>
              <el-input class="inputBox" v-model="attendID" placeholder="请输入警员编号" clearable maxlength="7"
                oninput="value=value.replace(/[^\d.]/g,'')" style="width: 220px;" show-word-limit />
            </td>
            <td>
              <el-text class="noteText" type="primary">出勤地区:</el-text>
              <el-input class="inputBox" v-model="attendAddress" style="width: 250px;" placeholder="请输入出勤地区" clearable />
            </td>
          </tr>
          <tr>
            <td>
              <el-text class="noteText" type="primary">出勤时间:</el-text>
              <el-date-picker class="inputBox" v-model="attendTime" type="date" placeholder="请选择出勤日期"
                style="margin-left: 10px;" />
            </td>
            <td>
              <el-text class="noteText" type="primary">是否筛选时间:</el-text>
              <el-select class="inputBox" v-model="isT" style="width: 200px;" placeholder="请选择">
                <el-option label="不筛选" value="F"></el-option>
                <el-option label="筛选" value="T"></el-option>
              </el-select>
            </td>
          </tr>
        </table>

        <div class="seaButton">
          <el-button type="primary" @click="fetchAttendInfo">查询</el-button>
        </div>
      </div>

      <el-divider />

      <div>
        <!-- 表格显示获取的出勤信息 -->
        <el-table :data="attendInfo" stripe height="450" @wheel.passive.stop>
          <el-table-column prop="id" label="出勤警员编号" sortable />
          <el-table-column prop="time" label="出勤时间" sortable />
          <el-table-column prop="area" label="出勤地区" sortable />
        </el-table>
      </div>
    </el-tab-pane>

    <!-- 新增栏 -->
    <el-tab-pane label="新增记录" name="2">
      <div style="text-align: center;">
        <table style="display: inline-block;text-align: left;">
          <tr>
            <td>
              <el-text class="noteText" type="primary">出勤警员编号:</el-text>
              <el-input class="inputBox" v-model="addAttendID" placeholder="请输入警员编号" clearable maxlength="7"
                oninput="value=value.replace(/[^\d.]/g,'')" style="width: 220px;" show-word-limit />
            </td>
            <td>
              <el-text class="noteText" type="primary">出勤地区:</el-text>
              <el-input class="inputBox" v-model="addAttendAddress" style="width: 250px;" placeholder="请输入出勤地区"
                clearable />
            </td>
          </tr>
          <tr>
            <td>
              <el-text class="noteText" type="primary">出勤时间:</el-text>
              <el-date-picker class="inputBox" v-model="addAttendTime" type="datetime" placeholder="请选择出勤时间"
                style="margin-left: 10px;" />
            </td>
            <td>
            </td>
          </tr>
        </table>

        <div class="seaButton">
          <el-button type="primary" @click="addAttendInfo">上传记录</el-button>
        </div>
      </div>
    </el-tab-pane>

    <!-- 删除栏 -->
    <el-tab-pane label="删除记录" name="3">
      <div style="text-align: center;">
        <table style="display: inline-block;text-align: left;">
          <tr>
            <td>
              <el-text class="noteText" type="primary">出勤警员编号:</el-text>
              <el-input class="inputBox" v-model="delAttendID" placeholder="请输入警员编号" clearable maxlength="7"
                oninput="value=value.replace(/[^\d.]/g,'')" style="width: 220px;" show-word-limit />
            </td>
            <td>
              <el-text class="noteText" type="primary">出勤地区:</el-text>
              <el-input class="inputBox" v-model="delAttendAddress" style="width: 250px;" placeholder="请输入出勤地区"
                clearable />
            </td>
          </tr>
          <tr>
            <td>
              <el-text class="noteText" type="primary">出勤时间:</el-text>
              <el-date-picker class="inputBox" v-model="delAttendTime" type="datetime" placeholder="请选择出勤时间"
                style="margin-left: 10px;" />
            </td>
            <td>
            </td>
          </tr>
        </table>

        <div class="seaButton">
          <el-button type="primary" @click="delAttendInfo">删除记录</el-button>
        </div>
      </div>
    </el-tab-pane>
  </el-tabs>
</template>

<script>
import axios from "../../api/request";

import { ElMessage } from 'element-plus';

export default {
  data() {
    return {
      anv: "1",
      attendID: "",
      attendAddress: "",
      attendTime: new Date(),
      isT: "F",
      attendInfo: [],
      addAttendID: "",
      addAttendAddress: "",
      addAttendTime: new Date(),
      delAttendID: "",
      delAttendAddress: "",
      delAttendTime: new Date(),
    }
  },
  methods: {
    handleMouseMove(event) {
      const x = event.pageX - event.target.offsetLeft;
      const y = event.pageY - event.target.offsetTop;

      event.target.style.setProperty("--x", `${x}px`);
      event.target.style.setProperty("--y", `${y}px`);
    },
    async fetchAttendInfo() {
      try {
        this.attendTime = new Date(this.attendTime).toISOString();
        const response = await axios.get(`/api/attendInfo?attendID=${encodeURIComponent(this.attendID)}&attendAddress=${encodeURIComponent(this.attendAddress)}&attendTime=${encodeURIComponent(this.attendTime)}&isT=${encodeURIComponent(this.isT)}`);
        for (let i = 0; i < response.data.length; i++) {
          // 将 time 中的 'T' 替换为空格
          response.data[i].time = response.data[i].time.replace('T', ' ');
        }
        this.attendInfo = response.data;
      } catch (error) {
        // 请求失败时的处理逻辑
        ElMessage({
          showClose: true,
          message: "获取出勤列表失败！",
          type: 'error',
          duration: 5000,
        });
      }
    },
    async addAttendInfo() {
      try {
        this.addAttendTime = new Date(this.addAttendTime).toISOString();
        const response = await axios.post(`/api/addAttend?attendID=${encodeURIComponent(this.addAttendID)}&attendAddress=${encodeURIComponent(this.addAttendAddress)}&attendTime=${encodeURIComponent(this.addAttendTime)}`);
        if (typeof response.data == "string") {
          ElMessage({
            showClose: true,
            message: response.data,
            type: 'warning',
            duration: 5000,
          });
        }
        else {
          await this.fetchAttendInfo();
          ElMessage({
            showClose: true,
            message: '新增记录成功! ',
            type: 'success',
            duration: 5000,
          });
        }
      } catch (error) {
        // 请求失败时的处理逻辑
        ElMessage({
          showClose: true,
          message: "新增记录失败！",
          type: 'error',
          duration: 5000,
        });
      }
    },
    async delAttendInfo() {
      try {
        this.delAttendTime = new Date(this.delAttendTime).toISOString();
        const response = await axios.delete(`/api/delAttend?attendID=${encodeURIComponent(this.delAttendID)}&attendAddress=${encodeURIComponent(this.delAttendAddress)}&attendTime=${encodeURIComponent(this.delAttendTime)}`);
        if (typeof response.data == "string") {
          ElMessage({
            showClose: true,
            message: response.data,
            type: 'warning',
            duration: 5000,
          });
        }
        else {
          await this.fetchAttendInfo();
          ElMessage({
            showClose: true,
            message: '删除出勤成功! ',
            type: 'success',
            duration: 5000,
          });
        }
      } catch (error) {
        // 请求失败时的处理逻辑
        ElMessage({
          showClose: true,
          message: "删除出勤失败！",
          type: 'error',
          duration: 5000,
        });
      }
    },
  },
  async created() {
    await this.fetchAttendInfo();
  },
};
</script>

<style lang="postcss" scoped>
.sub-header {
  overflow: hidden;
  display: flex;
  position: absolute;
  top: 70px;
  left: 199px;
  width: calc(100% - 199px);
  height: 7vh;
  min-height: 40px;
  align-items: center;
  /* 文字竖直方向居中对齐 */
  background-color: #1f2cdf;
  box-shadow: inset -500px 0px 200px 0px rgba(4, 0, 113, 0.856);
  color: #ffffff;
  font-size: 28px;
}

.sub-header::before {
  --size: 0;
  content: '';
  position: absolute;
  left: var(--x);
  top: var(--y);
  width: var(--size);
  height: var(--size);
  background: radial-gradient(circle closest-side, #5a65ff, transparent);
  transform: translate(-50%, -50%);
  transition: width .2s ease, height .2s ease;
}

.sub-header:hover::before {
  --size: 400px;
}

.noteText {
  font-size: 18px;
  text-align: center;
  margin: 20px 0px;
  width: 10vw;
  display: inline-block;
}

.inputBox {
  display: inline-block;
  width: 250px;
  height: 32px;
  margin: 20px 10px;
}

.seaButton {
  display: block;
  text-align: center;
  margin-top: 15px;
  margin-bottom: 10px;
}
</style>