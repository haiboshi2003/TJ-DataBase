<template>
  <el-header class="sub-header" @mousemove="handleMouseMove">
    <div>&nbsp;&nbsp;装备管理&nbsp;>&nbsp;警械管理</div>
  </el-header>

  <el-tabs v-model="anv" type="border-card" style="margin-top: 10vh;">
    <!-- 查询警械栏 -->
    <el-tab-pane label="查询警械" name="1">
      <div style="text-align: center;">
        <table style="display: inline-block;text-align: left;">
          <tr>
            <td>
              <el-text class="noteText" type="primary">警械编号：</el-text>
              <el-input class="inputBox" v-model="equipId" placeholder="请输入警械编号"
                oninput="value=value.replace(/[^\d.]/g,'')" clearable maxlength="7" show-word-limit />
            </td>
            <td>
              <el-text class="noteText" type="primary">警械类型：</el-text>
              <el-select class="inputBox" v-model="equipType" placeholder="请选择">
                <el-option selected label="全部类型" value=""></el-option>
                <el-option label="催泪弹" value="催泪弹"></el-option>
                <el-option label="手枪" value="手枪"></el-option>
                <el-option label="手铐" value="手铐"></el-option>
                <el-option label="警棍" value="警棍"></el-option>
                <el-option label="防弹背心" value="防弹背心"></el-option>
              </el-select>
            </td>
          </tr>
          <tr>
            <td>
              <el-text class="noteText" type="primary">警局编号：</el-text>
              <el-input class="inputBox" v-model="equipStation" placeholder="请输入警局编号"
                oninput="value=value.replace(/[^\d.]/g,'')" clearable maxlength="9" show-word-limit />
            </td>
            <td>
              <el-text class="noteText" type="primary">使用状态：</el-text>
              <el-select class="inputBox" v-model="equipStatus" placeholder="请选择">
                <el-option selected label="全部状态" value=""></el-option>
                <el-option label="使用中" value="1"></el-option>
                <el-option label="空闲" value="0"></el-option>
              </el-select>
            </td>
          </tr>
        </table>
        <div class="seaButton">
          <el-button type="primary" @click="fetchEquipInfo">查询</el-button>
        </div>
      </div>

      <el-divider />
      <div>
        <!-- 表格显示获取的警车信息 -->
        <el-table v-if="showEquip" :data="chanEquipInfo" stripe height="450" @wheel.passive.stop>
          <el-table-column prop="equipID" label="警械编号" sortable />
          <el-table-column prop="equipType" label="警械类型" sortable />
          <el-table-column prop="equipStation" label="所属警局编号" sortable />
          <el-table-column prop="equipStatus" label="是否借出" sortable />
          <el-table-column align="right" width="100px">
            <template #header></template>
            <template #default="scope">
              <el-button size="small" type="primary" @click="equipUseRecord(scope.$index, scope.row)">使用记录</el-button>
            </template>
          </el-table-column>
        </el-table>
        <!-- 使用记录列表 -->
        <el-table v-if="!showEquip && equipUseInfo.length > 0" :data="equipUseInfo" stripe height="450"
          @wheel.passive.stop>
          <el-table-column prop="equipID" label="警车编号" sortable />
          <el-table-column prop="policemenID" label="使用的警员编号" sortable />
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

    <!-- 新增警械栏 -->
    <el-tab-pane label="新增警械" name="2">
      <div style="text-align: center;">
        <table style="display: inline-block;text-align: left;">
          <tr>
            <td>
              <el-text class="noteText" type="primary">警械编号：</el-text>
              <el-input class="inputBox" v-model="addEquipID" placeholder="请输入警械编号"
                oninput="value=value.replace(/[^\d.]/g,'')" clearable maxlength="7" show-word-limit />
            </td>
            <td>
              <el-text class="noteText" type="primary">警械类型：</el-text>
              <el-select class="inputBox" v-model="addEquipType" placeholder="请选择">
                <el-option selected label="请选择" value=""></el-option>
                <el-option label="催泪弹" value="催泪弹"></el-option>
                <el-option label="手枪" value="手枪"></el-option>
                <el-option label="手铐" value="手铐"></el-option>
                <el-option label="警棍" value="警棍"></el-option>
                <el-option label="防弹背心" value="防弹背心"></el-option>
              </el-select>
            </td>
          </tr>
          <tr>
            <td>
              <el-text class="noteText" type="primary">警局编号：</el-text>
              <el-input class="inputBox" v-model="addEquipStation" placeholder="请输入警局编号"
                oninput="value=value.replace(/[^\d.]/g,'')" clearable maxlength="9" show-word-limit />
            </td>
            <td></td>
          </tr>
        </table>

        <div class="seaButton">
          <el-button type="primary" @click="addEquipInfo">上传警械信息</el-button>
        </div>
      </div>
    </el-tab-pane>

    <!-- 删除警车栏 -->
    <el-tab-pane label="删除警械" name="3">
      <div style="text-align: center;">
        <table style="display: inline-block;text-align: left;">
          <tr>
            <td>
              <el-text class="noteText" type="primary">警械编号：</el-text>
              <el-input class="inputBox" v-model="delEquipID" placeholder="请输入警械编号"
                oninput="value=value.replace(/[^\d.]/g,'')" clearable maxlength="7" show-word-limit />
            </td>
            <td>
              <el-button type="primary" @click="delEquipInfo" style="margin-left: 20px">删除警械</el-button>
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
              <el-text class="noteText" type="primary">警械编号：</el-text>
              <el-input class="inputBox" v-model="addEquipIDU" placeholder="请输入警械编号"
                oninput="value=value.replace(/[^\d.]/g,'')" clearable maxlength="7" show-word-limit />
            </td>
            <td>
              <el-text class="noteText" type="primary">警员编号：</el-text>
              <el-input class="inputBox" v-model="addPolicemenIDU" placeholder="请输入警员编号"
                oninput="value=value.replace(/[^\d.]/g,'')" clearable maxlength="7" show-word-limit />
            </td>
          </tr>
        </table>
        <div class="seaButton">
          <el-button type="primary" @click="addEquipUseRecord">登记使用</el-button>
        </div>
      </div>
    </el-tab-pane>

    <!-- 归还栏 -->
    <el-tab-pane label="归还登记" name="5">
      <div style="text-align: center;">
        <table style="display: inline-block;text-align: left;">
          <tr>
            <td>
              <el-text class="noteText" type="primary">警械编号：</el-text>
              <el-input class="inputBox" v-model="addEquipIDR" placeholder="请输入警械编号"
                oninput="value=value.replace(/[^\d.]/g,'')" clearable maxlength="7" show-word-limit />
            </td>
            <td>
              <el-text class="noteText" type="primary">警员编号：</el-text>
              <el-input class="inputBox" v-model="addPolicemenIDR" placeholder="请输入警员编号"
                oninput="value=value.replace(/[^\d.]/g,'')" clearable maxlength="7" show-word-limit />
            </td>
          </tr>
        </table>
        <div class="seaButton">
          <el-button type="primary" @click="returnEquip">登记归还</el-button>
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
      showEquip: true,
      equipId: '',
      equipType: '',
      equipStation: '',
      equipStatus: '',
      equipInfo: [],
      chanEquipInfo: [],
      equipUseInfo: [],
      addEquipID: "",
      addEquipType: "",
      addEquipStation: "",
      delEquipID: "",
      addEquipIDU: "",
      addPolicemenIDU: "",
      addEquipIDR: "",
      addPolicemenIDR: "",
    }
  },
  methods: {
    handleMouseMove(event) {
      const x = event.pageX - event.target.offsetLeft;
      const y = event.pageY - event.target.offsetTop;

      event.target.style.setProperty("--x", `${x}px`);
      event.target.style.setProperty("--y", `${y}px`);
    },
    async fetchEquipInfo() {
      try {
        const response = await axios.get(`/api/EquipList?equipId=${encodeURIComponent(this.equipId)}&equipType=${encodeURIComponent(this.equipType)}&equipStation=${encodeURIComponent(this.equipStation)}&equipStatus=${encodeURIComponent(this.equipStatus)}`);
        this.equipInfo = response.data;
        this.chanEquipInfo = this.equipInfo.map(item => {
          if (item.equipStatus === '0') {
            return { ...item, equipStatus: '空闲' };
          } else if (item.equipStatus === '1') {
            return { ...item, equipStatus: '使用中' };
          } else {
            return item;
          }
        });
      } catch (error) {
        // 请求失败时的处理逻辑
        ElMessage({
          showClose: true,
          message: "获取警械列表失败！",
          type: 'error',
          duration: 5000,
        });
      }
    },
    async addEquipInfo() {
      try {
        const response = await axios.post(`/api/addEquip?equipID=${encodeURIComponent(this.addEquipID)}&equipType=${encodeURIComponent(this.addEquipType)}&equipStation=${encodeURIComponent(this.addEquipStation)}`);
        if (typeof response.data == "string") {
          ElMessage({
            showClose: true,
            message: response.data,
            type: 'warning',
            duration: 5000,
          });
        }
        else {
          await this.fetchEquipInfo();
          ElMessage({
            showClose: true,
            message: '新增警械成功! ',
            type: 'success',
            duration: 5000,
          });
        }
      } catch (error) {
        // 请求失败时的处理逻辑
        ElMessage({
          showClose: true,
          message: "新增警械失败！",
          type: 'error',
          duration: 5000,
        });
      }
    },
    async delEquipInfo() {
      try {
        const response = await axios.delete(`/api/delEquip?equipID=${encodeURIComponent(this.delEquipID)}`);
        if (typeof response.data == "string") {
          ElMessage({
            showClose: true,
            message: response.data,
            type: 'warning',
            duration: 5000,
          });
        }
        else {
          await this.fetchEquipInfo();
          ElMessage({
            showClose: true,
            message: '删除警械成功! ',
            type: 'success',
            duration: 5000,
          });
        }
      } catch (error) {
        // 请求失败时的处理逻辑
        ElMessage({
          showClose: true,
          message: "删除警械失败！",
          type: 'error',
          duration: 5000,
        });
      }
    },
    async equipUseRecord(a, equip) {
      try {
        const response = await axios.get(`/api/equipUseInfo?equipID=${encodeURIComponent(equip.equipID)}`);
        for (let i = 0; i < response.data.length; i++) {
          response.data[i].borrowtime = response.data[i].borrowtime.replace('T', ' ');
          if (response.data[i].returntime == null)
            response.data[i].returntime = "————";
          else
            response.data[i].returntime = response.data[i].returntime.replace('T', ' ');
        }
        this.equipUseInfo = response.data;
        if (response.data.length == 0) {
          ElMessage({
            showClose: true,
            message: "该警械暂无使用记录",
            duration: 5000,
          });
        }
        else
          this.showEquip = false;
      } catch (error) {
        // 请求失败时的处理逻辑
        ElMessage({
          showClose: true,
          message: "获取警械使用记录失败！",
          type: 'error',
          duration: 5000,
        });
      }
    },
    async back() {
      this.equipUseInfo = "";
      this.showEquip = true;
    },
    async addEquipUseRecord() {
      try {
        const response = await axios.post(`/api/addEquipUseRecord?equipID=${encodeURIComponent(this.addEquipIDU)}&policemenID=${encodeURIComponent(this.addPolicemenIDU)}`);
        if (typeof response.data == "string") {
          ElMessage({
            showClose: true,
            message: response.data,
            type: 'warning',
            duration: 5000,
          });
        }
        else {
          await this.fetchEquipInfo();
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
    async returnEquip() {
      try {
        const response = await axios.put(`/api/returnEquip?equipID=${encodeURIComponent(this.addEquipIDR)}&policemenID=${encodeURIComponent(this.addPolicemenIDR)}`);
        if (typeof response.data == "string") {
          ElMessage({
            showClose: true,
            message: response.data,
            type: 'warning',
            duration: 5000,
          });
        }
        else {
          await this.fetchEquipInfo();
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
    await this.fetchEquipInfo();
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