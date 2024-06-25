<template>
  <el-header class="sub-header" @mousemove="handleMouseMove">
    <div>&nbsp;&nbsp;警员管理&nbsp;>&nbsp;接警记录管理</div>
  </el-header>

  <el-tabs v-model="anv" type="border-card" style="margin-top: 10vh;">
    <!-- 查询接警记录栏 -->
    <el-tab-pane label="查询接警记录" name="1">
      <div style="text-align: center;">
        <table style="display: inline-block;text-align: left;">
          <tr>
            <td>
              <el-text class="noteText" type="primary">接警记录编号:</el-text>
              <el-input class="inputBox" v-model="alarmID" placeholder="请输入接警记录编号"
                oninput="value=value.replace(/[^\d.]/g,'')" style="width: 220px" clearable maxlength="7"
                show-word-limit />
            </td>
            <td>
              <el-text class="noteText" type="primary">来电号码:</el-text>
              <el-input class="inputBox" v-model="alarmNum" placeholder="请输入来电号码"
                oninput="value=value.replace(/[^\d.]/g,'')" clearable maxlength="11" show-word-limit />
            </td>
          </tr>
          <tr>
            <td>
              <el-text class="noteText" type="primary">接警日期:</el-text>
              <el-date-picker class="inputBox" v-model="alarmTime" type="date" placeholder="请选择接警日期"
                style="margin-left: 10px;width:220px" />
            </td>
            <td>
              <el-text class="noteText" type="primary">是否筛选时间:</el-text>
              <el-select class="inputBox" v-model="isT" style="margin-left: 10px;width: 200px;" placeholder="请选择">
                <el-option label="不筛选" value="F"></el-option>
                <el-option label="筛选" value="T"></el-option>
              </el-select>
            </td>
          </tr>
          <tr>
            <td>
              <el-text class="noteText" type="primary">涉及案件编号:</el-text>
              <el-input v-model="caseID" placeholder="请输入案件编号" oninput="value=value.replace(/[^\d.]/g,'')" clearable
                maxlength="7" show-word-limit style="width: 220px; margin-left: 10px;">
                <template #prepend>
                  <div class="qianru">{{ type1 }}</div>
                </template></el-input>
            </td>
            <td>
              <el-text class="noteText" type="primary">案件类型:</el-text>
              <el-select class="inputBox" v-model="caseType" style="margin-left: 10px;width: 200px;" placeholder="请选择">
                <el-option label="全部类型" value=""></el-option>
                <el-option label="强奸" value="强奸"></el-option>
                <el-option label="抢劫" value="抢劫"></el-option>
                <el-option label="故意伤害" value="故意伤害"></el-option>
                <el-option label="盗窃" value="盗窃"></el-option>
                <el-option label="诈骗" value="诈骗"></el-option>
                <el-option label="谋杀" value="谋杀"></el-option>
              </el-select>
            </td>
          </tr>
          <tr>
            <td>
              <el-text class="noteText" type="primary">接警警员编号:</el-text>
              <el-input class=" inputBox" v-model="policemenID" placeholder="请输入警员编号"
                oninput="value=value.replace(/[^\d.]/g,'')" style="width: 220px" clearable maxlength="7"
                show-word-limit />
            </td>
            <td></td>
          </tr>
        </table>
        <div class="seaButton">
          <el-button type="primary" @click="fetchAlarmInfo">查询</el-button>
        </div>
      </div>

      <el-divider />
      <div>
        <!-- 表格显示获取的接警记录信息 -->
        <el-table :data="alarmInfo" stripe height="450" @wheel.passive.stop>
          <el-table-column prop="alarmID" label="接警记录编号" sortable width="150px" />
          <el-table-column prop="alarmTime" label="接警记录时间" sortable />
          <el-table-column prop="alarmNum" label="来电号码" sortable width="170px" />
          <el-table-column prop="caseID" label="涉及案件" sortable width="150px" />
          <el-table-column prop="policemenID" label="接警警员" sortable width="150px" />
        </el-table>
      </div>
    </el-tab-pane>

    <!-- 新增接警记录栏 -->
    <el-tab-pane label="新增接警记录" name="2">
      <div style="text-align: center;">
        <table style="display: inline-block;text-align: left;">
          <tr>
            <td>
              <el-text class="noteText" type="primary">接警记录编号:</el-text>
              <el-input class="inputBox" v-model="addAlarmID" placeholder="请输入接警记录编号"
                oninput="value=value.replace(/[^\d.]/g,'')" clearable maxlength="7" show-word-limit />
            </td>
            <td>
              <el-text class="noteText" type="primary">来电号码:</el-text>
              <el-input class="inputBox" v-model="addAlarmNum" placeholder="请输入来电号码"
                oninput="value=value.replace(/[^\d.]/g,'')" clearable maxlength="11" show-word-limit />
            </td>
          </tr>
          <tr>
            <td>
              <el-text class="noteText" type="primary">接警警员编号:</el-text>
              <el-input class="inputBox" v-model="addPolicemenID" placeholder="请输入警员编号"
                oninput="value=value.replace(/[^\d.]/g,'')" clearable maxlength="7" show-word-limit />
            </td>
            <td></td>
          </tr>
        </table>

        <div class="seaButton">
          <el-button type="primary" @click="addAlarmInfo">上传接警记录</el-button>
        </div>
      </div>
    </el-tab-pane>

    <!-- 删除接警记录栏 -->
    <el-tab-pane label="删除接警记录" name="3">
      <div style="text-align: center;">
        <table style="display: inline-block;text-align: left;">
          <tr>
            <td>
              <el-text class="noteText" type="primary">接警记录编号:</el-text>
              <el-input class="inputBox" v-model="delAlarmID" placeholder="请输入接警记录编号"
                oninput="value=value.replace(/[^\d.]/g,'')" clearable maxlength="7" show-word-limit />
            </td>
            <td>
              <el-button type="primary" @click="delAlarmInfo" style="margin-left: 20px">删除接警记录</el-button>
            </td>
          </tr>
        </table>
      </div>
    </el-tab-pane>

    <!-- 关联案件栏 -->
    <el-tab-pane label="添加/修改关联案件" name="4">
      <div style="text-align: center;">
        <table style="display: inline-block;text-align: left;">
          <tr>
            <td>
              <el-text class="noteText" type="primary">涉及案件编号:</el-text>
              <el-input v-model="relCaseID" placeholder="请输入案件编号" oninput="value=value.replace(/[^\d.]/g,'')" clearable
                maxlength="7" show-word-limit style="width: 220px; margin-left: 10px;">
                <template #prepend>
                  <div class="qianru">{{ type2 }}</div>
                </template></el-input>
            </td>
            <td>
              <el-text class="noteText" type="primary">案件类型:</el-text>
              <el-select class="inputBox" v-model="relCaseType" style="margin-left: 10px;width: 200px;" placeholder="请选择">
                <el-option label="强奸" value="强奸"></el-option>
                <el-option label="抢劫" value="抢劫"></el-option>
                <el-option label="故意伤害" value="故意伤害"></el-option>
                <el-option label="盗窃" value="盗窃"></el-option>
                <el-option label="诈骗" value="诈骗"></el-option>
                <el-option label="谋杀" value="谋杀"></el-option>
              </el-select>
            </td>
          </tr>
          <tr>
            <td>
              <el-text class="noteText" type="primary">接警记录编号:</el-text>
              <el-input class="inputBox" v-model="relAlarmID" placeholder="请输入接警记录编号"
                oninput="value=value.replace(/[^\d.]/g,'')" style="width: 220px" clearable maxlength="7"
                show-word-limit />
            </td>
            <td>
            </td>
          </tr>
        </table>
        <div class="seaButton">
          <el-button type="primary" @click="relAlarmInfo">关联案件</el-button>
        </div>
      </div>
    </el-tab-pane>

    <!-- 修改栏 -->
    <el-tab-pane label="修改接警记录" name="5">
      <div style="text-align: center;">
        <table style="display: inline-block;text-align: left;">
          <tr>
            <td>
              <el-text class="noteText" type="primary">接警记录编号:</el-text>
              <el-input class="inputBox" v-model="updAlarmID" placeholder="请输入接警记录编号"
                oninput="value=value.replace(/[^\d.]/g,'')" clearable maxlength="7" show-word-limit />
            </td>
            <td>
              <el-text class="noteText" type="primary">更新来电号码:</el-text>
              <el-input class="inputBox" v-model="updAlarmNum" placeholder="请输入来电号码"
                oninput="value=value.replace(/[^\d.]/g,'')" clearable maxlength="11" show-word-limit />
            </td>
          </tr>
          <tr>
            <td>
              <el-text class="noteText" type="primary">更新警员编号:</el-text>
              <el-input class="inputBox" v-model="updPolicemenID" placeholder="请输入警员编号"
                oninput="value=value.replace(/[^\d.]/g,'')" clearable maxlength="7" show-word-limit />
            </td>
            <td>
              <el-text class="noteText" type="primary">更新接警时间:</el-text>
              <el-date-picker class="inputBox" v-model="updAlarmTime" type="datetime" placeholder="请选择接警时间"
                style="margin-left: 10px;width:200px" />
            </td>
          </tr>
        </table>

        <div class="seaButton">
          <el-button type="primary" @click="updAlarmInfo">更新接警记录</el-button>
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
      alarmID: "",
      alarmNum: "",
      alarmTime: new Date(),
      isT: "F",
      caseID: "",
      caseType: "",
      policemenID: "",
      alarmInfo: [],
      type1: "",
      addAlarmID: "",
      addAlarmNum: "",
      addCaseID: "",
      addCaseType: "",
      addPolicemenID: "",
      delAlarmID: "",
      relAlarmID: "",
      relCaseID: "",
      relCaseType: "",
      type2: "",
      updAlarmID: "",
      updAlarmTime: new Date(),
      updAlarmNum: "",
      updPolicemenID: "",
    }
  },
  methods: {
    handleMouseMove(event) {
      const x = event.pageX - event.target.offsetLeft;
      const y = event.pageY - event.target.offsetTop;

      event.target.style.setProperty("--x", `${x}px`);
      event.target.style.setProperty("--y", `${y}px`);
    },
    async fetchAlarmInfo() {
      try {
        this.alarmTime = new Date(this.alarmTime).toISOString();
        const response = await axios.get(`/api/alarmInfo?alarmID=${encodeURIComponent(this.alarmID)}&alarmTime=${encodeURIComponent(this.alarmTime)}&isT=${encodeURIComponent(this.isT)}&alarmNum=${encodeURIComponent(this.alarmNum)}&caseID=${encodeURIComponent(this.caseID)}&caseType=${encodeURIComponent(this.caseType)}&policemenID=${encodeURIComponent(this.policemenID)}`);

        if (typeof response.data == "string") {
          ElMessage({
            showClose: true,
            message: response.data,
            type: 'warning',
            duration: 5000,
          });
        }
        else {
          for (let i = 0; i < response.data.length; i++) {
            // 将 alarmTime 中的 'T' 替换为空格
            response.data[i].alarmTime = response.data[i].alarmTime.replace('T', ' ');
          }
          this.alarmInfo = response.data;
        }
      } catch (error) {
        // 请求失败时的处理逻辑
        ElMessage({
          showClose: true,
          message: "获取接警记录列表失败！",
          type: 'error',
          duration: 5000,
        });
      }
    },
    async addAlarmInfo() {
      try {
        const response = await axios.post(`/api/addAlarm?alarmID=${encodeURIComponent(this.addAlarmID)}&alarmNum=${encodeURIComponent(this.addAlarmNum)}&policemenID=${encodeURIComponent(this.addPolicemenID)}`);
        if (typeof response.data == "string") {
          ElMessage({
            showClose: true,
            message: response.data,
            type: 'warning',
            duration: 5000,
          });
        }
        else {
          await this.fetchAlarmInfo();
          ElMessage({
            showClose: true,
            message: '新增接警记录成功! ',
            type: 'success',
            duration: 5000,
          });
        }
      } catch (error) {
        // 请求失败时的处理逻辑
        ElMessage({
          showClose: true,
          message: "新增接警记录失败！",
          type: 'error',
          duration: 5000,
        });
      }
    },
    async delAlarmInfo() {
      try {
        const response = await axios.delete(`/api/delAlarm?alarmID=${encodeURIComponent(this.delAlarmID)}`);
        if (typeof response.data == "string") {
          ElMessage({
            showClose: true,
            message: response.data,
            type: 'warning',
            duration: 5000,
          });
        }
        else {
          await this.fetchAlarmInfo();
          ElMessage({
            showClose: true,
            message: '删除接警记录成功! ',
            type: 'success',
            duration: 5000,
          });
        }
      } catch (error) {
        // 请求失败时的处理逻辑
        ElMessage({
          showClose: true,
          message: "删除接警记录失败！",
          type: 'error',
          duration: 5000,
        });
      }
    },
    async updAlarmInfo() {
      try {
        this.updAlarmTime = new Date(this.updAlarmTime).toISOString();
        const response = await axios.put(`/api/updAlarm?alarmID=${encodeURIComponent(this.updAlarmID)}&alarmTime=${encodeURIComponent(this.updAlarmTime)}&alarmNum=${encodeURIComponent(this.updAlarmNum)}&policemenID=${encodeURIComponent(this.updPolicemenID)}`);
        if (typeof response.data == "string") {
          ElMessage({
            showClose: true,
            message: response.data,
            type: 'warning',
            duration: 5000,
          });
        }
        else {
          await this.fetchAlarmInfo();
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
    async relAlarmInfo() {
      try {
        const response = await axios.put(`/api/relAlarm?alarmID=${encodeURIComponent(this.relAlarmID)}&caseID=${encodeURIComponent(this.relCaseID)}&caseType=${encodeURIComponent(this.relCaseType)}`);
        if (typeof response.data == "string") {
          ElMessage({
            showClose: true,
            message: response.data,
            type: 'warning',
            duration: 5000,
          });
        }
        else {
          await this.fetchAlarmInfo();
          ElMessage({
            showClose: true,
            message: '关联成功! ',
            type: 'success',
            duration: 5000,
          });
        }
      } catch (error) {
        // 请求失败时的处理逻辑
        ElMessage({
          showClose: true,
          message: "关联失败！",
          type: 'error',
          duration: 5000,
        });
      }
    },
    typeToID(type) {
      if (type === "") {
        return ("");
      }
      else if (type === "强奸") {
        return ("A");
      }
      else if (type === "抢劫") {
        return ("R");
      }
      else if (type === "故意伤害") {
        return ("H");
      }
      else if (type === "盗窃") {
        return ("T");
      }
      else if (type === "诈骗") {
        return ("C");
      }
      else if (type === "谋杀") {
        return ("M");
      }
      else {
        ElMessage({
          showClose: true,
          message: "案件类型转换错误！",
          type: 'error',
          duration: 5000,
        });
        return ("");
      }
    },
  },
  watch: {
    caseType() {
      this.type1 = this.typeToID(this.caseType);
    },
    relCaseType() {
      this.type2 = this.typeToID(this.relCaseType);
    },
  },
  async created() {
    await this.fetchAlarmInfo();
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

.qianru {
  width: 7px;
}
</style>