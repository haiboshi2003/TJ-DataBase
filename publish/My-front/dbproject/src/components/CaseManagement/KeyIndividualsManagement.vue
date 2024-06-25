<template>
  <el-header class="sub-header" @mousemove="handleMouseMove">
    <div>&nbsp;&nbsp;案件管理&nbsp;>&nbsp;重点人员统计</div>
  </el-header>

  <div class="main">
    <!-- 表格 -->
    <el-table v-if="!isDetail" :data="repeatOffenderInfo" height="450" @wheel.passive.stop stripe style="width: 100%">
      <el-table-column label="身份证号码" prop="身份证号" sortable width="180px" />
      <el-table-column label="姓名" prop="姓名" sortable width="100px" />
      <el-table-column label="性别" prop="性别" sortable width="80px" />
      <el-table-column label="住址" prop="住址" sortable />
      <el-table-column align="right" width="300">
        <template #header>
          <table>
            <tr>
              <td><el-text type="primary" style="display: inline-block;">筛选身份证号码：</el-text></td>
              <td><el-input v-model="roID" size="small" placeholder="请输入身份证号码"
                  style="display: inline-block; width: 140px; height: 25px" maxlength="18"
                  oninput="value=value.replace(/[^\d.]/g,'')" /></td>
            </tr>
            <tr>
              <td><el-text type="primary" style="display: inline-block;">筛选姓名：</el-text></td>
              <td><el-input v-model="roName" size="small" placeholder="请输入姓名"
                  style="display: inline-block; width: 140px; height: 25px" /></td>
            </tr>
            <tr>
              <td><el-text type="primary" style="display: inline-block;">筛选性别：</el-text></td>
              <td>
                <el-select v-model="roSex" placeholder="请选择" size="small"
                  style="display: inline-block; width: 140px; height: 25px">
                  <el-option label="全部性别" value="全部" />
                  <el-option label="男" value="M" />
                  <el-option label="女" value="F" />
                </el-select>
              </td>
            </tr>
            <tr>
              <td><el-text type="primary" style="display: inline-block;">筛选住址：</el-text></td>
              <td><el-input v-model="roAddress" size="small" placeholder="请输入住址"
                  style="display: inline-block; width: 140px; height: 25px" /></td>
            </tr>
          </table>
        </template>
        <template #default="scope">
          <el-button size="small" type="primary" @click="getDetail(scope.$index, scope.row)">详情</el-button>
        </template>
      </el-table-column>
    </el-table>

    <el-table v-if="isDetail" :data="caseRecord" height="450" @wheel.passive.stop stripe style="width: 100%">
      <el-table-column label="案件编号" prop="案件编号" sortable width="110px" />
      <el-table-column label="案件类型" prop="案件类型" sortable width="110px" />
      <el-table-column label="案件状态" prop="案件状态" sortable width="110px" />
      <el-table-column label="登记时间" prop="登记时间" sortable />
      <el-table-column label="案发地点" prop="案发地点" sortable />
      <el-table-column label="案件等级" prop="案件等级" sortable width="110px" />
      <el-table-column width="120px">
        <template #header>
          <el-button type="primary" @click="goBack" size="small">返回重犯列表</el-button>
        </template>
      </el-table-column>
    </el-table>
  </div>
</template>

<script>
import axios from "../../api/request";

import { ElMessage } from 'element-plus';

export default {
  data() {
    return {
      repeatOffenderInfo: "",
      roName: "",
      isDetail: false,
      roID: "",
      roSex: "全部",
      roAddress: "",
      caseRecord: "",
    };
  },
  methods: {
    handleMouseMove(event) {
      const x = event.pageX - event.target.offsetLeft;
      const y = event.pageY - event.target.offsetTop;

      event.target.style.setProperty("--x", `${x}px`);
      event.target.style.setProperty("--y", `${y}px`);
    },
    async getRepeatOffenderInfo() {
      try {
        const response = await axios.get("/api/keyIndividualsStatistics/repeatOffenderInfoStatistics");
        this.repeatOffenderInfo = response.data;
      } catch (error) {
        // 请求失败时的处理逻辑
        ElMessage({
          showClose: true,
          message: "获取重犯列表失败！",
          type: 'error',
          duration: 5000,
        });
      }
    },
    goBack() {
      if (this.isDetail) {
        this.isDetail = false;
      }
    },
    async getDetail(index, man) {
      window.scrollTo({
        top: 0,
        behavior: 'smooth' // 可选的平滑滚动效果
      });
      this.isDetail = true;
      try {
        const response = await axios.get("/api/keyIndividualsStatistics/repeatOffenderCaseStatistics", { params: { id: man["身份证号"] } });
        for (let i = 0; i < response.data.length; i++) {
          if (response.data[i]["案件等级"] == "0")
            response.data[i]["案件等级"] = "一般";
          else if (response.data[i]["案件等级"] == "1")
            response.data[i]["案件等级"] = "较大";
          else if (response.data[i]["案件等级"] == "2")
            response.data[i]["案件等级"] = "重大";
          else if (response.data[i]["案件等级"] == "3")
            response.data[i]["案件等级"] = "特别重大";
        }
        this.caseRecord = response.data;
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
    async filter() {
      try {
        const response = await axios.get(`/api/keyIndividualsStatistics/repeatOffenderFilterStatistics?ID=${encodeURIComponent(this.roID)}&name=${encodeURIComponent(this.roName)}&sex=${encodeURIComponent(this.roSex)}&address=${encodeURIComponent(this.roAddress)}`);
        this.repeatOffenderInfo = response.data;
      } catch (error) {
        // 请求失败时的处理逻辑
        ElMessage({
          showClose: true,
          message: "信息失败！",
          type: 'error',
          duration: 5000,
        });
      }
    }
  },
  async created() {
    await this.getRepeatOffenderInfo();
  },
  watch: {
    async roID() {
      await this.filter();
    },
    async roName() {
      await this.filter();
    },
    async roSex() {
      await this.filter();
    },
    async roAddress() {
      await this.filter();
    },
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
</style>