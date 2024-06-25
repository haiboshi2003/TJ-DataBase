<template>
  <el-header class="sub-header" @mousemove="handleMouseMove">
    <div>&nbsp;&nbsp;警务处理系统&nbsp;>&nbsp;案件办理</div>
  </el-header>

  <el-table v-if="process === '待调查案件'" :data="cases" height="450" @wheel.passive.stop stripe
    style="width: 100%; margin-top: 10vh">
    <el-table-column label="案件编号" prop="caseID" sortable width="104px" />
    <el-table-column label="案件类型" prop="caseType" sortable width="104px" />
    <el-table-column label="案件状态" prop="status" sortable width="104px" />
    <el-table-column label="登记时间" prop="registerTime" width="105px" sortable />
    <el-table-column label="案发地点" prop="address" sortable />
    <el-table-column label="案件等级" prop="ranking" sortable width="104px" />
    <el-table-column align="right" width="268px">
      <template #header>
        <table>
          <tr>
            <td><el-text type="primary" style="display: inline-block;">筛选案件编号：</el-text></td>
            <td><el-input v-model="caseID" size="small" placeholder="请输入案件编号"
                style="display: inline-block; width: 130px; height: 25px" clearable maxlength="8" /></td>
          </tr>
        </table>
      </template>
      <template #default="scope">
        <el-button size="small" type="primary" @click="setTeam(scope.$index, scope.row)">进行调查</el-button>
      </template>
    </el-table-column>
  </el-table>

  <div v-if="process === '选择办案人员'" style="text-align: center">
    <el-table :data="policemen" height="400" @selection-change="selectPolicemen" @wheel.passive.stop stripe
      style="width: 100%; margin-top: 10vh">
      <el-table-column type="selection" />
      <el-table-column label="警员编号" prop="policeNum" sortable />
      <el-table-column label="姓名" prop="policeName" sortable />
      <el-table-column label="性别" prop="gender" sortable />
      <el-table-column label="民族" prop="nation" sortable />
      <el-table-column label="状态" prop="status" sortable />
      <el-table-column label="职位" prop="position" sortable />
      <el-table-column align="left" width="260px">
        <template #header>
          <table>
            <tr>
              <td><el-text type="primary" style="display: inline-block; width: 130px">当前选择{{ selectCaseID }}</el-text>
              </td>
              <td><el-button size="small" type="primary" @click="goBack">重选案件</el-button></td>
            </tr>
            <tr>
              <td><el-text type="primary" style="display: inline-block;">筛选警员编号：</el-text></td>
              <td><el-input v-model="policemenID" size="small" placeholder="请输入警员编号"
                  style="display: inline-block; width: 100px; height: 25px" /></td>
            </tr>
          </table>
        </template>
      </el-table-column>
    </el-table>

    <el-button type="primary" @click="finish" :disabled="isSet" style="margin-top: 10px">调查此案件</el-button>
  </div>

  <div v-if="process === '完成'" style="text-align: center">
    <el-result icon="success" title="案件已设为调查" sub-title="点击按钮返回立案列表">
      <template #extra>
        <el-button type="primary" @click="backToH">返回</el-button>
      </template>
    </el-result>
  </div>
</template>

<script>
import axios from "../api/request";

export default {
  data() {
    return {
      caseID: "",
      process: "待调查案件",
      cases: [],
      policemen: "",
      policemenID: "",
      selectCaseID: "",
      team: "",
      isSet: true,
    };
  },
  methods: {
    handleMouseMove(event) {
      const x = event.pageX - event.target.offsetLeft;
      const y = event.pageY - event.target.offsetTop;

      event.target.style.setProperty("--x", `${x}px`);
      event.target.style.setProperty("--y", `${y}px`);
    },
    async setTeam(index, Case) {
      this.process = "选择办案人员";
      await this.getPolicemen();
      this.selectCaseID = Case.caseID;
    },
    async getCases() {
      try {
        const response = await axios.get(`/api/caseInvestigation/queryCase?caseID=${encodeURIComponent(this.caseID)}`);

        for (let i = 0; i < response.data.length; i++) {
          // 将 alarmTime 中的 'T' 替换为空格
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
        this.cases = response.data;
      } catch (error) {
        // 请求失败时的处理逻辑
        ElMessage({
          showClose: true,
          message: "获取立案列表失败！",
          type: 'error',
          duration: 5000,
        });
      }
    },
    async getPolicemen() {
      try {
        const response = await axios.get(`/api/caseInvestigation/queryPolicemen?policemenID=${encodeURIComponent(this.policemenID)}`);
        this.policemen = response.data;
      } catch (error) {
        // 请求失败时的处理逻辑
        ElMessage({
          showClose: true,
          message: "获取警员列表失败！",
          type: 'error',
          duration: 5000,
        });
      }
    },
    goBack() {
      this.process = "待调查案件";
      this.selectCaseID = "";
    },
    selectPolicemen(selection) {
      this.team = selection;
      if (this.team.length > 0) {
        this.isSet = false;
      }
      else
        this.isSet = true;

    },
    async finish() {
      try {
        await axios.put(`/api/caseInvestigation/modifyCaseStatus?caseID=${encodeURIComponent(this.selectCaseID)}`);
        this.process = "完成";
        this.selectCaseID = "";
        this.team = "";
        await this.getCases();
      } catch (error) {
        // 请求失败时的处理逻辑
        ElMessage({
          showClose: true,
          message: "进行调查失败！",
          type: 'error',
          duration: 5000,
        });
      };


    },
    backToH() {
      this.process = "待调查案件";
    }
  },
  async created() {
    await this.getCases();
  },
  watch: {
    async caseID() {
      await this.getCases();
    },
    async policemenID() {
      await this.getPolicemen();
    }
  }
};
</script>

<style>
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

.main {
  margin-top: 10vh;
}

.caseContainer {
  width: 200pt;
  background-color: rgb(20, 0, 98);
  color: white;
  position: relative;
}

.policeContainer {
  width: 200pt;
  background-color: rgb(98, 0, 0);
  color: white;
  position: relative;
}

.title {
  text-align: center;
}
</style>
