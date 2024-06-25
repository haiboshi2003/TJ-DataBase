<template>
  <el-header class="sub-header" @mousemove="handleMouseMove">
    <div>&nbsp;&nbsp;警员管理&nbsp;>&nbsp;警局信息管理</div>
  </el-header>

  <el-tabs v-model="anv" type="border-card" style="margin-top: 10vh;">
    <!-- 查询栏 -->
    <el-tab-pane label="查询警局" name="1">
      <div style="text-align: center;">
        <table style="display: inline-block;text-align: left;">
          <tr>
            <td>
              <el-text class="noteText" type="primary">警局编号:</el-text>
              <el-input class="inputBox" v-model="stationID" placeholder="请输入警局编号" clearable maxlength="9"
                oninput="value=value.replace(/[^\d.]/g,'')" show-word-limit />
            </td>
            <td>
              <el-text class="noteText" type="primary">警局名称:</el-text>
              <el-input class="inputBox" v-model="stationName" placeholder="请输入警局名称" clearable />
            </td>
          </tr>
          <tr>
            <td>
              <el-text class="noteText" type="primary">所在地址:</el-text>
              <el-input class="inputBox" v-model="stationAddress" placeholder="请输入地址" clearable />
            </td>
            <td></td>
          </tr>
        </table>

        <div class="seaButton">
          <el-button type="primary" @click="fetchStationInfo">查询</el-button>
        </div>
      </div>

      <el-divider />

      <div>
        <!-- 表格显示获取的警局信息 -->
        <el-table :data="stationInfo" stripe height="450" @wheel.passive.stop>
          <el-table-column prop="stationID" label="警局编号" sortable />
          <el-table-column prop="stationName" label="警局名称" sortable />
          <el-table-column prop="address" label="所在地址" sortable />
          <el-table-column prop="budget" label="预算" sortable />
        </el-table>
      </div>
    </el-tab-pane>

    <!-- 新增栏 -->
    <el-tab-pane label="新增警局" name="2">
      <div style="text-align: center;">
        <table style="display: inline-block;text-align: left;">
          <tr>
            <td>
              <el-text class="noteText" type="primary">警局编号:</el-text>
              <el-input class="inputBox" v-model="addStationID" placeholder="请输入警局编号" clearable maxlength="9"
                oninput="value=value.replace(/[^\d.]/g,'')" show-word-limit />
            </td>
            <td>
              <el-text class="noteText" type="primary">警局名称:</el-text>
              <el-input class="inputBox" v-model="addStationName" placeholder="请输入警局名称" clearable />
            </td>
          </tr>
          <tr>
            <td>
              <el-text class="noteText" type="primary">所在地址:</el-text>
              <el-input class="inputBox" v-model="addStationAddress" placeholder="请输入地址" clearable />
            </td>
            <td>
              <el-text class="noteText" type="primary">预算:</el-text>
              <el-input class="inputBox" v-model="addStationBudget" placeholder="请输入预算"
                oninput="value=value.replace(/[^\d.]/g,'')" clearable />
            </td>
          </tr>
        </table>

        <div class="seaButton">
          <el-button type="primary" @click="addStationInfo">上传警局信息</el-button>
        </div>
      </div>
    </el-tab-pane>

    <!-- 删除栏 -->
    <el-tab-pane label="删除警局" name="3">
      <div style="text-align: center;">
        <table style="display: inline-block;text-align: left;">
          <tr>
            <td>
              <el-text class="noteText" type="primary">警局编号：</el-text>
              <el-input class="inputBox" v-model="delStationID" placeholder="请输入警局编号" clearable maxlength="9"
                oninput="value=value.replace(/[^\d.]/g,'')" show-word-limit />
            </td>
            <td>
              <el-button type="primary" @click="delStationInfo" style="margin-left: 20px">删除警局</el-button>
            </td>
          </tr>
        </table>
      </div>
    </el-tab-pane>

    <!-- 修改栏 -->
    <el-tab-pane label="修改警局信息" name="4">
      <div style="text-align: center;">
        <table style="display: inline-block;text-align: left;">
          <tr>
            <td>
              <el-text class="noteText" type="primary">警局编号:</el-text>
              <el-input class="inputBox" v-model="updStationID" placeholder="请输入警局编号" clearable maxlength="9"
                oninput="value=value.replace(/[^\d.]/g,'')" show-word-limit />
            </td>
            <td>
              <el-text class="noteText" type="primary">更新警局名称:</el-text>
              <el-input class="inputBox" v-model="updStationName" placeholder="请输入警局名称" clearable />
            </td>
          </tr>
          <tr>
            <td>
              <el-text class="noteText" type="primary">更新所在地址:</el-text>
              <el-input class="inputBox" v-model="updStationAddress" placeholder="请输入地址" clearable />
            </td>
            <td>
              <el-text class="noteText" type="primary">更新预算:</el-text>
              <el-input class="inputBox" v-model="updStationBudget" placeholder="请输入预算"
                oninput="value=value.replace(/[^\d.]/g,'')" clearable />
            </td>
          </tr>
        </table>

        <div class="seaButton">
          <el-button type="primary" @click="updStationInfo">更新警局信息</el-button>
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
      stationID: '',
      stationName: '',
      stationAddress: '',
      stationBudget: "",
      stationInfo: [],
      addStationID: "",
      addStationName: "",
      addStationAddress: "",
      addStationBudget: "",
      delStationID: "",
      updStationID: "",
      updStationName: "",
      updStationAddress: "",
      updStationBudget: "",
      err: '警局不存在！',
    }
  },
  methods: {
    handleMouseMove(event) {
      const x = event.pageX - event.target.offsetLeft;
      const y = event.pageY - event.target.offsetTop;

      event.target.style.setProperty("--x", `${x}px`);
      event.target.style.setProperty("--y", `${y}px`);
    },
    async fetchStationInfo() {
      try {
        const response = await axios.get(`/api/stationInfo?stationID=${encodeURIComponent(this.stationID)}&stationName=${encodeURIComponent(this.stationName)}&stationAddress=${encodeURIComponent(this.stationAddress)}`);
        this.stationInfo = response.data;
      } catch (error) {
        // 请求失败时的处理逻辑
        ElMessage({
          showClose: true,
          message: "获取警局列表失败！",
          type: 'error',
          duration: 5000,
        });
      }
    },
    async addStationInfo() {
      try {
        const response = await axios.post(`/api/addStation?stationID=${encodeURIComponent(this.addStationID)}&stationName=${encodeURIComponent(this.addStationName)}&stationAddress=${encodeURIComponent(this.addStationAddress)}&stationBudget=${encodeURIComponent(this.addStationBudget)}`);
        if (typeof response.data == "string") {
          ElMessage({
            showClose: true,
            message: response.data,
            type: 'warning',
            duration: 5000,
          });
        }
        else {
          await this.fetchStationInfo();
          ElMessage({
            showClose: true,
            message: '新增警局成功! ',
            type: 'success',
            duration: 5000,
          });
        }
      } catch (error) {
        // 请求失败时的处理逻辑
        ElMessage({
          showClose: true,
          message: "新增警局失败！",
          type: 'error',
          duration: 5000,
        });
      }
    },
    async delStationInfo() {
      try {
        const response = await axios.delete(`/api/delStation?stationID=${encodeURIComponent(this.delStationID)}`);
        if (typeof response.data == "string") {
          ElMessage({
            showClose: true,
            message: response.data,
            type: 'warning',
            duration: 5000,
          });
        }
        else {
          await this.fetchStationInfo();
          ElMessage({
            showClose: true,
            message: '删除警局成功! ',
            type: 'success',
            duration: 5000,
          });
        }
      } catch (error) {
        // 请求失败时的处理逻辑
        ElMessage({
          showClose: true,
          message: "删除警局失败！",
          type: 'error',
          duration: 5000,
        });
      }
    },
    async updStationInfo() {
      try {
        const response = await axios.put(`/api/updStation?stationID=${encodeURIComponent(this.updStationID)}&stationName=${encodeURIComponent(this.updStationName)}&stationAddress=${encodeURIComponent(this.updStationAddress)}&stationBudget=${encodeURIComponent(this.updStationBudget)}`);
        if (typeof response.data == "string") {
          ElMessage({
            showClose: true,
            message: response.data,
            type: 'warning',
            duration: 5000,
          });
        }
        else {
          await this.fetchStationInfo();
          ElMessage({
            showClose: true,
            message: '修改成功! ',
            type: 'success',
            duration: 5000,
          });
        }
      } catch (error) {
        // 请求失败时的处理逻辑
        ElMessage({
          showClose: true,
          message: "修改失败！",
          type: 'error',
          duration: 5000,
        });
      }
    },
    async back() {
      this.stationUseInfo = "";
      this.showStation = true;
    },
  },
  async created() {
    await this.fetchStationInfo();
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
  width: 200px;
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