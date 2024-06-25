<template>
  <el-header class="sub-header" @mousemove="handleMouseMove">
    <div>&nbsp;&nbsp;案件管理&nbsp;>&nbsp;受害人信息管理</div>
  </el-header>

  <el-tabs v-model="anv" type="border-card" style="margin-top: 10vh;">
    <!-- 查询受害人栏 -->
    <el-tab-pane label="查询受害人" name="1">
      <div style="text-align: center;">
        <table style="display: inline-block;text-align: left;">
          <tr>
            <td>
              <el-text class="noteText" type="primary">姓名：</el-text>
              <el-input class="inputBox" v-model="victimName" placeholder="请输入姓名" clearable />
            </td>
            <td>
              <el-text class="noteText" type="primary">性别：</el-text>
              <el-select class="inputBox" v-model="victimSex" placeholder="请选择" style="width: 235px">
                <el-option selected label="全部性别" value=""></el-option>
                <el-option label="男" value="M"></el-option>
                <el-option label="女" value="F"></el-option>
              </el-select>
            </td>
          </tr>
          <tr>
            <td>
              <el-text class="noteText" type="primary">住址：</el-text>
              <el-input class="inputBox" v-model="victimAddress" placeholder="请输入住址" clearable />
            </td>
            <td>
              <el-text class="noteText" type="primary">身份证号：</el-text>
              <el-input class="inputBox" v-model="victimID" placeholder="请输入身份证号码"
                oninput="value=value.replace(/[^\d.]/g,'')" clearable maxlength="18" show-word-limit
                style="width: 235px" />
            </td>
          </tr>
        </table>
        <div class="seaButton">
          <el-button type="primary" @click="fetchVictimInfo">查询</el-button>
        </div>
      </div>

      <el-divider />
      <div>
        <!-- 表格显示获取的受害人信息 -->
        <el-table v-if="!showCase" :data="chanVictimInfo" stripe height="450" @wheel.passive.stop>
          <el-table-column prop="id" label="身份证号" sortable width="250px" />
          <el-table-column prop="name" label="姓名" sortable width="150px" />
          <el-table-column prop="sex" label="性别" sortable width="150px" />
          <el-table-column prop="address" label="住址" sortable />
          <el-table-column align="right" width="100px">
            <template #header></template>
            <template #default="scope">
              <el-button size="small" type="primary" @click="getCase(scope.$index, scope.row)">涉及案件</el-button>
            </template>
          </el-table-column>
        </el-table>
        <!-- 案件列表 -->
        <el-table v-if="showCase && caseInfo.length > 0" :data="caseInfo" stripe height="450" @wheel.passive.stop>
          <el-table-column prop="id" label="案件编号" sortable width="120px" />
          <el-table-column prop="type" label="案件类型" sortable width="120px" />
          <el-table-column prop="status" label="案件状态" sortable width="120px" />
          <el-table-column prop="registerTime" label="登记时间" sortable />
          <el-table-column prop="address" label="案发地点" sortable />
          <el-table-column prop="ranking" label="案件等级" sortable width="120px" />
          <el-table-column align="center" width="80px">
            <template #header>
              <el-button size="small" type="primary" @click="back">返回</el-button>
            </template>
          </el-table-column>
        </el-table>
      </div>
    </el-tab-pane>

    <!-- 新增受害人栏 -->
    <el-tab-pane label="新增受害人" name="2">
      <div style="text-align: center;">
        <table style="display: inline-block;text-align: left;">
          <tr>
            <td>
              <el-text class="noteText" type="primary" style="margin-right: 10px;">案件编号:</el-text>
              <el-input v-model="addCaseID" placeholder="案件编号" clearable maxlength="7"
                oninput="value=value.replace(/[^\d.]/g,'')" show-word-limit style="width: 190px;">
                <template #prepend>
                  <div class="qianru">{{ type1 }}</div>
                </template>
              </el-input>
            </td>
            <td>
              <el-text class="noteText" type="primary">案件类型:</el-text>
              <el-select class="inputBox" v-model="addCaseType" placeholder="请选择" style="width: 235px">
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
            </td>
            <td>
              <el-text class="noteText" type="primary" style="margin-left: 7px;">身份证号：</el-text>
              <el-input class="inputBox" v-model="addVictimID" placeholder="请输入身份证号"
                oninput="value=value.replace(/[^\d.]/g,'')" style="margin-left: 3px; width: 235px" clearable
                maxlength="18" show-word-limit />
            </td>
          </tr>
        </table>

        <div class="seaButton">
          <el-button type="primary" @click="addVictimInfo">新增受害人</el-button>
        </div>
      </div>
    </el-tab-pane>

    <!-- 删除受害人栏 -->
    <el-tab-pane label="删除受害人" name="3">
      <div style="text-align: center;">
        <table style="display: inline-block;text-align: left;">
          <tr>
            <td>
              <el-text class="noteText" type="primary" style="margin-right: 10px;">案件编号:</el-text>
              <el-input v-model="delCaseID" placeholder="案件编号" clearable maxlength="7"
                oninput="value=value.replace(/[^\d.]/g,'')" show-word-limit style="width: 190px;">
                <template #prepend>
                  <div class="qianru">{{ type2 }}</div>
                </template>
              </el-input>
            </td>
            <td>
              <el-text class="noteText" type="primary">案件类型:</el-text>
              <el-select class="inputBox" v-model="delCaseType" placeholder="请选择" style="width: 235px">
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
            </td>
            <td>
              <el-text class="noteText" type="primary" style="margin-left: 7px;">身份证号：</el-text>
              <el-input class="inputBox" v-model="delVictimID" placeholder="请输入身份证号"
                oninput="value=value.replace(/[^\d.]/g,'')" style="margin-left: 3px; width: 235px" clearable
                maxlength="18" show-word-limit />
            </td>
          </tr>
        </table>

        <div class="seaButton">
          <el-button type="primary" @click="delVictimInfo">删除受害人</el-button>
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
      showCase: false,
      victimID: '',
      victimName: '',
      victimSex: '',
      victimAddress: '',
      victimInfo: [],
      chanVictimInfo: [],
      caseInfo: [],
      addVictimID: "",
      addCaseID: "",
      addCaseType: "",
      type1: "",
      delVictimID: "",
      delCaseID: "",
      delCaseType: "",
      type2: "",
    }
  },
  methods: {
    handleMouseMove(event) {
      const x = event.pageX - event.target.offsetLeft;
      const y = event.pageY - event.target.offsetTop;

      event.target.style.setProperty("--x", `${x}px`);
      event.target.style.setProperty("--y", `${y}px`);
    },
    async fetchVictimInfo() {
      try {
        const response = await axios.get(`/api/victimInfo?victimID=${encodeURIComponent(this.victimID)}&victimName=${encodeURIComponent(this.victimName)}&victimSex=${encodeURIComponent(this.victimSex)}&victimAddress=${encodeURIComponent(this.victimAddress)}`);
        this.victimInfo = response.data;
        this.chanVictimInfo = this.victimInfo.map(item => {
          if (item.sex === 'M') {
            return { ...item, sex: '男' };
          } else if (item.sex === 'F') {
            return { ...item, sex: '女' };
          } else {
            return item;
          }
        });
      } catch (error) {
        // 请求失败时的处理逻辑
        ElMessage({
          showClose: true,
          message: "获取受害人列表失败！",
          type: 'error',
          duration: 5000,
        });
      }
    },
    async getCase(a, victim) {
      try {
        const response = await axios.get(`/api/getCaseV?victimID=${encodeURIComponent(victim.id)}`);
        for (let i = 0; i < response.data.length; i++) {
          // 将 registerTime 中的 'T' 替换为空格
          response.data[i].registerTime = response.data[i].registerTime.replace('T', ' ');
          if (response.data[i].ranking == "0")
            response.data[i].ranking = "一般";
          else if (response.data[i].ranking == "1")
            response.data[i].ranking = "较大";
          else if (response.data[i].ranking == "2")
            response.data[i].ranking = "重大";
          else if (response.data[i].ranking == "3")
            response.data[i].ranking = "特别重大";
        }
        this.caseInfo = response.data;
        this.showCase = true;
      } catch (error) {
        // 请求失败时的处理逻辑
        ElMessage({
          showClose: true,
          message: "获取案件信息失败！",
          type: 'error',
          duration: 5000,
        });
      }
    },
    async addVictimInfo() {
      try {
        const response = await axios.post(`/api/addVictim?victimID=${encodeURIComponent(this.addVictimID)}&caseID=${encodeURIComponent(this.addCaseID)}&caseType=${encodeURIComponent(this.addCaseType)}`);
        if (typeof response.data == "string") {
          ElMessage({
            showClose: true,
            message: response.data,
            type: 'warning',
            duration: 5000,
          });
        }
        else {
          await this.fetchVictimInfo();
          ElMessage({
            showClose: true,
            message: '新增受害人成功! ',
            type: 'success',
            duration: 5000,
          });
        }
      } catch (error) {
        // 请求失败时的处理逻辑
        ElMessage({
          showClose: true,
          message: "新增受害人失败！",
          type: 'error',
          duration: 5000,
        });
      }
    },
    async delVictimInfo() {
      try {
        const response = await axios.delete(`/api/delVictim?victimID=${encodeURIComponent(this.delVictimID)}&caseID=${encodeURIComponent(this.delCaseID)}&caseType=${encodeURIComponent(this.delCaseType)}`);
        if (typeof response.data == "string") {
          ElMessage({
            showClose: true,
            message: response.data,
            type: 'warning',
            duration: 5000,
          });
        }
        else {
          await this.fetchVictimInfo();
          ElMessage({
            showClose: true,
            message: '删除受害人成功! ',
            type: 'success',
            duration: 5000,
          });
        }
      } catch (error) {
        // 请求失败时的处理逻辑
        ElMessage({
          showClose: true,
          message: "删除受害人失败！",
          type: 'error',
          duration: 5000,
        });
      }
    },
    async back() {
      this.caseInfo = "";
      this.showCase = false;
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
    addCaseType() {
      this.type1 = this.typeToID(this.addCaseType);
    },
    delCaseType() {
      this.type2 = this.typeToID(this.delCaseType);
    },
  },
  async created() {
    await this.fetchVictimInfo();
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
  width: 250px;
  height: 32px;
  margin: 20px 10px;
}

.qianru {
  width: 7px;
}
</style>