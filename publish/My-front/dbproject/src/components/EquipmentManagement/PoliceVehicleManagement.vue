<template>
  <el-header class="sub-header" @mousemove="handleMouseMove">
    <div>&nbsp;&nbsp;装备管理&nbsp;>&nbsp;警车管理</div>
  </el-header>

  <el-tabs v-model="anv" type="border-card" style="margin-top: 10vh;">
    <!-- 查询警车栏 -->
    <el-tab-pane label="查询警车" name="1">
      <div style="text-align: center;">
        <table style="display: inline-block;text-align: left;">
          <tr>
            <td>
              <el-text class="noteText" type="primary">警车编号：</el-text>
              <el-input class="inputBox" v-model="vehicleId" placeholder="请输入警车编号" clearable maxlength="6"
                oninput="value=value.replace(/[^\d.]/g,'')" show-word-limit />
            </td>
            <td>
              <el-text class="noteText" type="primary">警车类型：</el-text>
              <el-select class="inputBox" v-model="vehicletp" placeholder="请选择">
                <el-option selected label="全部类型" value=""></el-option>
                <el-option label="小轿车" value="小轿车"></el-option>
                <el-option label="两座摩托" value="两座摩托"></el-option>
                <el-option label="大型巴士" value="大型巴士"></el-option>
                <el-option label="大巴车" value="大巴车"></el-option>
                <el-option label="摩托车" value="摩托车"></el-option>
              </el-select>
            </td>
          </tr>
          <tr>
            <td>
              <el-text class="noteText" type="primary">使用状态：</el-text>
              <el-select class="inputBox" v-model="vehiclest" placeholder="请选择">
                <el-option selected label="全部状态" value=""></el-option>
                <el-option label="使用中" value="1"></el-option>
                <el-option label="空闲" value="0"></el-option>
              </el-select>
            </td>
            <td></td>
          </tr>
        </table>
        <div class="seaButton">
          <el-button type="primary" @click="fetchVehicleInfo">查询</el-button>
        </div>
      </div>

      <el-divider />
      <div>
        <!-- 表格显示获取的警车信息 -->
        <el-table v-if="showVehicle" :data="chanVehicleInfo" stripe height="450" @wheel.passive.stop>
          <el-table-column prop="vehicle_ID" label="警车编号" sortable />
          <el-table-column prop="vehicle_Type" label="警车类型" sortable />
          <el-table-column prop="status" label="是否借出" sortable />
          <el-table-column align="right" width="100px">
            <template #header></template>
            <template #default="scope">
              <el-button size="small" type="primary" @click="vehicleuseInfo(scope.$index, scope.row)">使用记录</el-button>
            </template>
          </el-table-column>
        </el-table>
        <!-- 使用记录列表 -->
        <el-table v-if="!showVehicle && vehicleUseInfo.length > 0" :data="vehicleUseInfo" stripe height="450"
          @wheel.passive.stop>
          <el-table-column prop="vehicleID" label="警车编号" sortable />
          <el-table-column prop="stationID" label="使用的警局编号" sortable />
          <el-table-column prop="borrowtime" label="借出时间" sortable />
          <el-table-column prop="returntime" label="还回时间" sortable />
          <el-table-column align="center" width="80px">
            <template #header>
              <el-button size="small" type="primary" @click="back">返回</el-button>
            </template>
          </el-table-column>
        </el-table>
      </div>
    </el-tab-pane>

    <!-- 新增警车栏 -->
    <el-tab-pane label="新增警车" name="2">
      <div style="text-align: center;">
        <table style="display: inline-block;text-align: left;">
          <tr>
            <td>
              <el-text class="noteText" type="primary">警车编号：</el-text>
              <el-input class="inputBox" v-model="addVehicleID" placeholder="请输入警车编号"
                oninput="value=value.replace(/[^\d.]/g,'')" clearable maxlength="6" show-word-limit />
            </td>
            <td>
              <el-text class="noteText" type="primary">警车类型：</el-text>
              <el-select class="inputBox" v-model="addVehicleType" placeholder="请选择">
                <el-option selected label="小轿车" value="小轿车"></el-option>
                <el-option label="两座摩托" value="两座摩托"></el-option>
                <el-option label="大型巴士" value="大型巴士"></el-option>
                <el-option label="大巴车" value="大巴车"></el-option>
                <el-option label="摩托车" value="摩托车"></el-option>
              </el-select>
            </td>
          </tr>
        </table>
        <div class="seaButton">
          <el-button type="primary" @click="addVehicleInfo">上传警车信息</el-button>
        </div>
      </div>
    </el-tab-pane>

    <!-- 删除警车栏 -->
    <el-tab-pane label="删除警车" name="3">
      <div style="text-align: center;">
        <table style="display: inline-block;text-align: left;">
          <tr>
            <td>
              <el-text class="noteText" type="primary">警车编号：</el-text>
              <el-input class="inputBox" v-model="delVehicleID" placeholder="请输入警车编号"
                oninput="value=value.replace(/[^\d.]/g,'')" clearable maxlength="6" show-word-limit />
            </td>
            <td>
              <el-button type="primary" @click="delVehicleInfo" style="margin-left: 20px">删除警车</el-button>
            </td>
          </tr>
        </table>
      </div>
    </el-tab-pane>

    <!-- 新增使用记录栏 -->
    <el-tab-pane label="使用登记" name="4">
      <div style="text-align: center;">
        <table style="display: inline-block;text-align: left;">
          <tr>
            <td>
              <el-text class="noteText" type="primary">警车编号：</el-text>
              <el-input class="inputBox" v-model="addVehicleIDU" placeholder="请输入警车编号"
                oninput="value=value.replace(/[^\d.]/g,'')" clearable maxlength="6" show-word-limit />
            </td>
            <td>
              <el-text class="noteText" type="primary">警局编号：</el-text>
              <el-input class="inputBox" v-model="addStationIDU" placeholder="请输入警局编号"
                oninput="value=value.replace(/[^\d.]/g,'')" clearable maxlength="9" show-word-limit />
            </td>
          </tr>
        </table>
        <div class="seaButton">
          <el-button type="primary" @click="addVehicleUseRecord">登记使用</el-button>
        </div>
      </div>
    </el-tab-pane>

    <!-- 归还栏 -->
    <el-tab-pane label="归还登记" name="5">
      <div style="text-align: center;">
        <table style="display: inline-block;text-align: left;">
          <tr>
            <td>
              <el-text class="noteText" type="primary">警车编号：</el-text>
              <el-input class="inputBox" v-model="addVehicleIDR" placeholder="请输入警车编号"
                oninput="value=value.replace(/[^\d.]/g,'')" clearable maxlength="6" show-word-limit />
            </td>
            <td>
              <el-text class="noteText" type="primary">警局编号：</el-text>
              <el-input class="inputBox" v-model="addStationIDR" placeholder="请输入警局编号"
                oninput="value=value.replace(/[^\d.]/g,'')" clearable maxlength="9" show-word-limit />
            </td>
          </tr>
        </table>
        <div class="seaButton">
          <el-button type="primary" @click="returnVehicle">登记归还</el-button>
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
      showVehicle: true,
      vehicleId: '',
      vehicletp: '',
      vehiclest: '',
      vehicleInfo: [],
      chanVehicleInfo: [],
      vehicleUseInfo: [],
      addVehicleID: "",
      addVehicleType: "",
      delVehicleID: "",
      addVehicleIDU: "",
      addStationIDU: "",
      addVehicleIDR: "",
      addStationIDR: "",
    }
  },
  methods: {
    handleMouseMove(event) {
      const x = event.pageX - event.target.offsetLeft;
      const y = event.pageY - event.target.offsetTop;

      event.target.style.setProperty("--x", `${x}px`);
      event.target.style.setProperty("--y", `${y}px`);
    },
    async fetchVehicleInfo() {
      try {
        const response = await axios.get(`/api/vehicles?VID=${encodeURIComponent(this.vehicleId)}&VTYPE=${encodeURIComponent(this.vehicletp)}&VST=${encodeURIComponent(this.vehiclest)}`);
        this.vehicleInfo = response.data;
        this.chanVehicleInfo = this.vehicleInfo.map(item => {
          if (item.status === '0') {
            return { ...item, status: '空闲' };
          } else if (item.status === '1') {
            return { ...item, status: '使用中' };
          } else {
            return item;
          }
        });
      } catch (error) {
        // 请求失败时的处理逻辑
        ElMessage({
          showClose: true,
          message: "获取警车列表失败！",
          type: 'error',
          duration: 5000,
        });
      }
    },
    async addVehicleInfo() {
      try {
        const response = await axios.post(`/api/addVehicle?vehicleID=${encodeURIComponent(this.addVehicleID)}&vehicleType=${encodeURIComponent(this.addVehicleType)}`);
        if (typeof response.data == "string") {
          ElMessage({
            showClose: true,
            message: response.data,
            type: 'warning',
            duration: 5000,
          });
        }
        else {
          await this.fetchVehicleInfo();
          ElMessage({
            showClose: true,
            message: '新增警车成功! ',
            type: 'success',
            duration: 5000,
          });
        }
      } catch (error) {
        // 请求失败时的处理逻辑
        ElMessage({
          showClose: true,
          message: "新增警车失败！",
          type: 'error',
          duration: 5000,
        });
      }
    },
    async delVehicleInfo() {
      try {
        const response = await axios.delete(`/api/delVehicle?vehicleID=${encodeURIComponent(this.delVehicleID)}`);
        if (typeof response.data == "string") {
          ElMessage({
            showClose: true,
            message: response.data,
            type: 'warning',
            duration: 5000,
          });
        }
        else {
          await this.fetchVehicleInfo();
          ElMessage({
            showClose: true,
            message: '删除警车成功! ',
            type: 'success',
            duration: 5000,
          });
        }
      } catch (error) {
        // 请求失败时的处理逻辑
        ElMessage({
          showClose: true,
          message: "删除警车失败！",
          type: 'error',
          duration: 5000,
        });
      }
    },
    async vehicleuseInfo(a, vehicle) {
      try {
        const response = await axios.get(`/api/vehiclesUseStatistics?VID=${encodeURIComponent(vehicle.vehicle_ID)}`);
        for (let i = 0; i < response.data.length; i++) {
          response.data[i].borrowtime = response.data[i].borrowtime.replace('T', ' ');
          if (response.data[i].returntime == null)
            response.data[i].returntime = "————";
          else
            response.data[i].returntime = response.data[i].returntime.replace('T', ' ');
        }
        this.vehicleUseInfo = response.data;
        if (response.data.length == 0) {
          ElMessage({
            showClose: true,
            message: "该警车暂无使用记录",
            duration: 5000,
          });
        }
        else
          this.showVehicle = false;
      } catch (error) {
        // 请求失败时的处理逻辑
        ElMessage({
          showClose: true,
          message: "获取警车使用记录失败！",
          type: 'error',
          duration: 5000,
        });
      }
    },
    async back() {
      this.vehicleUseInfo = "";
      this.showVehicle = true;
    },
    async addVehicleUseRecord() {
      try {
        const response = await axios.post(`/api/addVehicleUseRecord?vehicleID=${encodeURIComponent(this.addVehicleIDU)}&stationID=${encodeURIComponent(this.addStationIDU)}`);
        if (typeof response.data == "string") {
          ElMessage({
            showClose: true,
            message: response.data,
            type: 'warning',
            duration: 5000,
          });
        }
        else {
          await this.fetchVehicleInfo();
          ElMessage({
            showClose: true,
            message: '登记成功! ',
            type: 'success',
            duration: 5000,
          });
        }
      } catch (error) {
        // 请求失败时的处理逻辑
        ElMessage({
          showClose: true,
          message: "登记失败！",
          type: 'error',
          duration: 5000,
        });
      }
    },
    async returnVehicle() {
      try {
        const response = await axios.put(`/api/returnVehicle?vehicleID=${encodeURIComponent(this.addVehicleIDR)}&stationID=${encodeURIComponent(this.addStationIDR)}`);
        if (typeof response.data == "string") {
          ElMessage({
            showClose: true,
            message: response.data,
            type: 'warning',
            duration: 5000,
          });
        }
        else {
          await this.fetchVehicleInfo();
          ElMessage({
            showClose: true,
            message: '归还成功! ',
            type: 'success',
            duration: 5000,
          });
        }
      } catch (error) {
        // 请求失败时的处理逻辑
        ElMessage({
          showClose: true,
          message: "归还失败！",
          type: 'error',
          duration: 5000,
        });
      }
    },
  },
  async created() {
    await this.fetchVehicleInfo();
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

.seaButton {
  display: block;
  text-align: center;
  margin-top: 15px;
  margin-bottom: 10px;
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
</style>