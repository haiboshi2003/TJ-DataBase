<template>
  <el-header class="sub-header" @mousemove="handleMouseMove">
    <div>&nbsp;&nbsp;案件管理&nbsp;>&nbsp;案件分类与管理</div>
  </el-header>

  <el-tabs v-model="anv" type="border-card" style="margin-top: 10vh;">
    <!-- 查询栏 -->
    <el-tab-pane label="查询案件" name="1">
      <div style="text-align: center;">
        <table style="display: inline-block;text-align: left;">
          <tr>
            <td>
              <el-text class="noteText" type="primary" style="margin-right: 10px;">案件编号:</el-text>
              <el-input v-model="caseID" placeholder="案件编号" clearable maxlength="7"
                oninput="value=value.replace(/[^\d.]/g,'')" show-word-limit style="width: 200px;">
                <template #prepend>
                  <div class="qianru">{{ type1 }}</div>
                </template>
              </el-input>
            </td>
            <td>
              <el-text class="noteText" type="primary">案件类型:</el-text>
              <el-select class="inputBox" v-model="caseType" placeholder="请选择">
                <el-option selected label="全部类型" value=""></el-option>
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
            <td><el-text class="noteText" type="primary">案发地点:</el-text>
              <el-input class="inputBox" v-model="caseAddress" placeholder="请输入案发地点" clearable />
            </td>
            <td>
              <el-text class="noteText" type="primary">案件状态:</el-text>
              <el-select class="inputBox" v-model="caseStatus" placeholder="请选择">
                <el-option selected label="全部状态" value=""></el-option>
                <el-option label="立案" value="立案"></el-option>
                <el-option label="调查" value="调查"></el-option>
                <el-option label="结案" value="结案"></el-option>
              </el-select>
            </td>
          </tr>
          <tr>
            <td></td>
            <td>
              <el-text class="noteText" type="primary">案件等级:</el-text>
              <el-select class="inputBox" v-model="caseRanking" placeholder="请选择">
                <el-option selected label="全部等级" value=""></el-option>
                <el-option label="一般" value="0"></el-option>
                <el-option label="较大" value="1"></el-option>
                <el-option label="重大" value="2"></el-option>
                <el-option label="特别重大" value="3"></el-option>
              </el-select>
            </td>
          </tr>
        </table>

        <div class="seaButton">
          <el-button type="primary" @click="fetchCaseInfo">查询</el-button>
        </div>
      </div>

      <el-divider />

      <div>
        <!-- 表格显示获取的案件信息 -->
        <el-table v-if="!isDetail" :data="caseInfo" stripe height="450" @wheel.passive.stop>
          <el-table-column prop="caseID" label="案件编号" sortable width="110px" />
          <el-table-column prop="caseType" label="案件类型" sortable width="110px" />
          <el-table-column prop="status" label="案件状态" sortable width="110px" />
          <el-table-column prop="registerTime" label="登记时间" sortable />
          <el-table-column prop="address" label="案发地点" sortable />
          <el-table-column prop="ranking" label="案件等级" sortable width="110px" />
          <el-table-column align="right" width="120px">
            <template #header>
            </template>
            <template #default="scope">
              <el-button size="small" type="primary" @click="getDetail(scope.$index, scope.row)">查看涉案人</el-button>
            </template>
          </el-table-column>
        </el-table>

        <el-table v-if="isDetail" :data="detCaseInfo" stripe height="450" @wheel.passive.stop>
          <el-table-column prop="name" label="姓名" sortable />
          <el-table-column prop="id" label="身份证号" sortable />
          <el-table-column prop="gender" label="性别" sortable />
          <el-table-column prop="caseT" label="涉案方式" sortable />
          <el-table-column width="80px">
            <template #header>
              <el-button size="small" type="primary" @click="back">返回</el-button>
            </template>
            <template #default="scope">
            </template>
          </el-table-column>
        </el-table>
      </div>
    </el-tab-pane>

    <!-- 结案栏 -->
    <el-tab-pane label="案件结案" name="2">
      <div style="text-align: center;">
        <table style="display: inline-block;text-align: left;">
          <tr>
            <td>
              <el-text class="noteText" type="primary" style="margin-right: 10px;">案件编号:</el-text>
              <el-input v-model="closeCaseID" placeholder="案件编号" clearable maxlength="7"
                oninput="value=value.replace(/[^\d.]/g,'')" show-word-limit style="width: 200px;">
                <template #prepend>
                  <div class="qianru">{{ type2 }}</div>
                </template>
              </el-input>
            </td>
          </tr>
          <tr>
            <td>
              <el-text class="noteText" type="primary">案件类型:</el-text>
              <el-select class="inputBox" v-model="closeCaseType" placeholder="请选择">
                <el-option label="强奸" value="强奸"></el-option>
                <el-option label="抢劫" value="抢劫"></el-option>
                <el-option label="故意伤害" value="故意伤害"></el-option>
                <el-option label="盗窃" value="盗窃"></el-option>
                <el-option label="诈骗" value="诈骗"></el-option>
                <el-option label="谋杀" value="谋杀"></el-option>
              </el-select>
            </td>
          </tr>
        </table>

        <div class="seaButton">
          <el-button type="primary" @click="closeCase" style="margin-left: 20px">提交结案</el-button>
        </div>
      </div>
    </el-tab-pane>

    <!-- 新增栏 -->
    <el-tab-pane label="新增案件记录" name="3">
      <div style="text-align: center;">
        <table style="display: inline-block;text-align: left;">
          <tr>
            <td>
              <el-text class="noteText" type="primary" style="margin-right: 10px;">案件编号:</el-text>
              <el-input v-model="addCaseID" placeholder="案件编号" clearable maxlength="7"
                oninput="value=value.replace(/[^\d.]/g,'')" show-word-limit style="width: 200px;">
                <template #prepend>
                  <div class="qianru">{{ type3 }}</div>
                </template>
              </el-input>
            </td>
            <td>
              <el-text class="noteText" type="primary">案件类型:</el-text>
              <el-select class="inputBox" v-model="addCaseType" placeholder="请选择">
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
            <td><el-text class="noteText" type="primary">案发地点:</el-text>
              <el-input class="inputBox" v-model="addCaseAddress" placeholder="请输入案发地点" clearable />
            </td>
            <td>
              <el-text class="noteText" type="primary">案件等级:</el-text>
              <el-select class="inputBox" v-model="addCaseRanking" placeholder="请选择">
                <el-option label="一般" value="0"></el-option>
                <el-option label="较大" value="1"></el-option>
                <el-option label="重大" value="2"></el-option>
                <el-option label="特别重大" value="3"></el-option>
              </el-select>
            </td>
          </tr>
        </table>

        <div class="seaButton">
          <el-button type="primary" @click="addCaseInfo">新建</el-button>
        </div>
      </div>
    </el-tab-pane>

    <!-- 删除栏 -->
    <el-tab-pane label="删除案件记录" name="4">
      <div style="text-align: center;">
        <table style="display: inline-block;text-align: left;">
          <tr>
            <td>
              <el-text class="noteText" type="primary" style="margin-right: 10px;">案件编号:</el-text>
              <el-input v-model="delCaseID" placeholder="案件编号" clearable maxlength="7"
                oninput="value=value.replace(/[^\d.]/g,'')" show-word-limit style="width: 200px;">
                <template #prepend>
                  <div class="qianru">{{ type4 }}</div>
                </template>
              </el-input>
            </td>
          </tr>
          <tr>
            <td>
              <el-text class="noteText" type="primary">案件类型:</el-text>
              <el-select class="inputBox" v-model="delCaseType" placeholder="请选择">
                <el-option label="强奸" value="强奸"></el-option>
                <el-option label="抢劫" value="抢劫"></el-option>
                <el-option label="故意伤害" value="故意伤害"></el-option>
                <el-option label="盗窃" value="盗窃"></el-option>
                <el-option label="诈骗" value="诈骗"></el-option>
                <el-option label="谋杀" value="谋杀"></el-option>
              </el-select>
            </td>
          </tr>
        </table>

        <div class="seaButton">
          <el-button type="primary" @click="delCaseInfo" style="margin-left: 20px">删除案件</el-button>
        </div>
      </div>
    </el-tab-pane>

    <!-- 修改栏 -->
    <el-tab-pane label="修改案件记录" name="5">
      <div style="text-align: center;">
        <table style="display: inline-block;text-align: left;">
          <tr>
            <td>
              <el-text class="noteText" type="primary" style="margin-right: 10px;">原案件编号:</el-text>
              <el-input v-model="oriCaseID" placeholder="案件编号" clearable maxlength="7"
                oninput="value=value.replace(/[^\d.]/g,'')" show-word-limit style="width: 200px;">
                <template #prepend>
                  <div class="qianru">{{ type6 }}</div>
                </template>
              </el-input>
            </td>
            <td>
              <el-text class="noteText" type="primary">原案件类型:</el-text>
              <el-select class="inputBox" v-model="oriCaseType" placeholder="请选择">
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
              <el-divider />
            </td>
            <td>
              <el-divider />
            </td>
          </tr>
          <tr>
            <td>
              <el-text class="noteText" type="primary" style="margin-right: 10px;">新案件编号:</el-text>
              <el-input v-model="updCaseID" placeholder="案件编号" clearable maxlength="7"
                oninput="value=value.replace(/[^\d.]/g,'')" show-word-limit style="width: 200px;">
                <template #prepend>
                  <div class="qianru">{{ type5 }}</div>
                </template>
              </el-input>
            </td>
            <td>
              <el-text class="noteText" type="primary">新案件类型:</el-text>
              <el-select class="inputBox" v-model="updCaseType" placeholder="请选择">
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
            <td><el-text class="noteText" type="primary">新案发地点:</el-text>
              <el-input class="inputBox" v-model="updCaseAddress" placeholder="请输入案发地点" clearable />
            </td>
            <td>
              <el-text class="noteText" type="primary">新案件等级:</el-text>
              <el-select class="inputBox" v-model="updCaseRanking" placeholder="请选择">
                <el-option label="一般" value="0"></el-option>
                <el-option label="较大" value="1"></el-option>
                <el-option label="重大" value="2"></el-option>
                <el-option label="特别重大" value="3"></el-option>
              </el-select>
            </td>
          </tr>
        </table>

        <div class="seaButton">
          <el-button type="primary" @click="updCaseInfo">修改信息</el-button>
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
      caseID: "",
      caseType: "",
      caseStatus: "",
      caseAddress: "",
      caseRanking: "",
      caseInfo: [],
      type1: "",
      isDetail: false,
      detCaseInfo: [],
      closeCaseID: "",
      closeCaseType: "",
      type2: "",
      addCaseID: "",
      addCaseType: "",
      addCaseAddress: "",
      addCaseRanking: "",
      type3: "",
      delCaseID: "",
      delCaseType: "",
      type4: "",
      updCaseID: "",
      updCaseType: "",
      updCaseStatus: "",
      updCaseAddress: "",
      updCaseRanking: "",
      type5: "",
      type6: "",
      oriCaseID: "",
      oriCaseType: "",
    }
  },
  methods: {
    handleMouseMove(event) {
      const x = event.pageX - event.target.offsetLeft;
      const y = event.pageY - event.target.offsetTop;

      event.target.style.setProperty("--x", `${x}px`);
      event.target.style.setProperty("--y", `${y}px`);
    },
    async fetchCaseInfo() {
      try {
        const response = await axios.get(`/api/caseInfo?caseID=${encodeURIComponent(this.caseID)}&caseType=${encodeURIComponent(this.caseType)}&caseStatus=${encodeURIComponent(this.caseStatus)}&caseAddress=${encodeURIComponent(this.caseAddress)}&caseRanking=${encodeURIComponent(this.caseRanking)}`);
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
      } catch (error) {
        // 请求失败时的处理逻辑
        ElMessage({
          showClose: true,
          message: "获取案件列表失败！",
          type: 'error',
          duration: 5000,
        });
      }
    },
    async getDetail(e, acase) {
      try {
        const response = await axios.get(`/api/caseDetail?caseID=${encodeURIComponent(acase.caseID)}`);
        if (response.data.length == 0) {
          ElMessage({
            showClose: true,
            message: "该案件暂无登记的涉案人",
            duration: 5000,
          });
        }
        else {
          this.isDetail = true;
          this.detCaseInfo = response.data;
        }
      } catch (error) {
        // 请求失败时的处理逻辑
        ElMessage({
          showClose: true,
          message: "获取涉案人失败！",
          type: 'error',
          duration: 5000,
        });
      }
    },
    async closeCase() {
      try {
        const response = await axios.put(`/api/closeCase?caseID=${encodeURIComponent(this.closeCaseID)}&caseType=${encodeURIComponent(this.closeCaseType)}`);
        if (typeof response.data == "string") {
          ElMessage({
            showClose: true,
            message: response.data,
            type: 'warning',
            duration: 5000,
          });
        }
        else {
          await this.fetchCaseInfo();
          ElMessage({
            showClose: true,
            message: '案件已结案! ',
            type: 'success',
            duration: 5000,
          });
        }
      } catch (error) {
        // 请求失败时的处理逻辑
        ElMessage({
          showClose: true,
          message: "结案失败！",
          type: 'error',
          duration: 5000,
        });
      }
    },
    async addCaseInfo() {
      try {
        const response = await axios.post(`/api/addCase?caseID=${encodeURIComponent(this.addCaseID)}&caseType=${encodeURIComponent(this.addCaseType)}&caseAddress=${encodeURIComponent(this.addCaseAddress)}&caseRanking=${encodeURIComponent(this.addCaseRanking)}`);
        if (typeof response.data == "string") {
          ElMessage({
            showClose: true,
            message: response.data,
            type: 'warning',
            duration: 5000,
          });
        }
        else {
          await this.fetchCaseInfo();
          ElMessage({
            showClose: true,
            message: '新增案件成功! ',
            type: 'success',
            duration: 5000,
          });
        }
      } catch (error) {
        // 请求失败时的处理逻辑
        ElMessage({
          showClose: true,
          message: "新增案件失败！",
          type: 'error',
          duration: 5000,
        });
      }
    },
    async delCaseInfo() {
      try {
        const response = await axios.delete(`/api/delCase?caseID=${encodeURIComponent(this.delCaseID)}&caseType=${encodeURIComponent(this.delCaseType)}`);
        if (typeof response.data == "string") {
          ElMessage({
            showClose: true,
            message: response.data,
            type: 'warning',
            duration: 5000,
          });
        }
        else {
          await this.fetchCaseInfo();
          ElMessage({
            showClose: true,
            message: '删除案件成功! ',
            type: 'success',
            duration: 5000,
          });
        }
      } catch (error) {
        // 请求失败时的处理逻辑
        ElMessage({
          showClose: true,
          message: "删除案件失败！",
          type: 'error',
          duration: 5000,
        });
      }
    },
    async updCaseInfo() {
      try {
        const response = await axios.put(`/api/updCase?oriCaseID=${encodeURIComponent(this.oriCaseID)}&oriCaseType=${encodeURIComponent(this.oriCaseType)}&updCaseID=${encodeURIComponent(this.updCaseID)}&updCaseType=${encodeURIComponent(this.updCaseType)}&updCaseAddress=${encodeURIComponent(this.updCaseAddress)}&updCaseRanking=${encodeURIComponent(this.updCaseRanking)}`);
        if (typeof response.data == "string") {
          ElMessage({
            showClose: true,
            message: response.data,
            type: 'warning',
            duration: 5000,
          });
        }
        else {
          await this.fetchCaseInfo();
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
      this.detCaseInfo = "";
      this.isDetail = false;
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
    closeCaseType() {
      this.type2 = this.typeToID(this.closeCaseType);
    },
    addCaseType() {
      this.type3 = this.typeToID(this.addCaseType);
    },
    delCaseType() {
      this.type4 = this.typeToID(this.delCaseType);
    },
    updCaseType() {
      this.type5 = this.typeToID(this.updCaseType);
    },
    oriCaseType() {
      this.type6 = this.typeToID(this.oriCaseType);
    },
  },
  async created() {
    await this.fetchCaseInfo();
  }
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

.qianru {
  width: 7px;
}
</style>