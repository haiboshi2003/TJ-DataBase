<template>
  <el-header class="sub-header" @mousemove="handleMouseMove">
    <div>&nbsp;&nbsp;城区和居民管理&nbsp;>&nbsp;城市调度管理</div>
  </el-header>
  <main class="main">
    <section>
      <el-tabs v-model="anv" type="border-card" style="margin-top: 10vh">
        <el-tab-pane
          v-if="process === '选择调度警员'"
          label="查询警员信息"
          name="1"
        >
          <div style="text-align: center">
            <table style="display: inline-block; text-align: left">
              <!-- 这里开始输入 -->
              <tr>
                <td>
                  <el-text class="noteText" type="primary">输入警员ID:</el-text>
                  <el-input
                    class="inputBox"
                    v-model="policemenNumber"
                    placeholder="请输入警员编号"
                    clearable
                    maxlength="7"
                    oninput="value=value.replace(/[^\d.]/g,'')"
                    style="width: 231px"
                    show-word-limit
                  />
                </td>
                <td>
                  <el-text class="noteText" type="primary"
                    >输入警员姓名:</el-text
                  >
                  <el-input
                    class="inputBox"
                    v-model="policemenName"
                    style="width: 250px"
                    placeholder="请输入姓名"
                    clearable
                  />
                </td>
              </tr>

              <tr>
                <td>
                  <el-text class="noteText" type="primary"
                    >选择工作状态:</el-text
                  >
                  <el-select
                    class="inputBox"
                    v-model="policemenStatus"
                    style="width: 231px"
                    placeholder="请选择"
                  >
                    <el-option label="全部状态" selected value=""></el-option>
                    <el-option label="在职" value="在职"></el-option>
                    <el-option label="离职" value="离职"></el-option>
                  </el-select>
                </td>
                <td>
                  <el-text class="noteText" type="primary">选择职务:</el-text>
                  <el-select
                    class="inputBox"
                    v-model="policemenPosition"
                    style="width: 200px"
                    placeholder="请选择"
                  >
                    <el-option label="全部职务" selected value=""></el-option>
                    <el-option label="学员" value="学员"></el-option>
                    <el-option label="警员" value="警员"></el-option>
                    <el-option label="警司" value="警司"></el-option>
                    <el-option label="警督" value="警督"></el-option>
                    <el-option label="警监" value="警监"></el-option>
                    <el-option label="总警监" value="总警监"></el-option>
                  </el-select>
                </td>
              </tr>
            </table>
            <!-- 输入结束，下面是按钮 -->
            <div class="seaButton">
              <el-button type="primary" @click="fetchpolicemenInfo"
                >查询</el-button
              >
            </div>
          </div>
          <el-divider />

          <!-- 按钮结束，表格显示获取的警员信息 -->
          <el-table
            v-if="policemenInfos.length > 0"
            :data="policemenInfos"
            stripe
            height="450"
            @wheel.passive.stop
          >
            <el-table-column
              prop="pid"
              label="警员编号"
              sortable
              width="105px"
            />
            <el-table-column prop="name" label="姓名" sortable width="80px" />
            <el-table-column
              prop="idn"
              label="身份证号"
              sortable
              width="175px"
            />
            <el-table-column
              prop="birthday"
              label="出生日期"
              sortable
              width="105px"
            />
            <el-table-column prop="gender" label="性别" sortable width="80px" />
            <el-table-column prop="nation" label="民族" sortable width="80px" />
            <el-table-column prop="phone" label="电话" sortable width="115px" />
            <el-table-column prop="email" label="邮箱" sortable />
            <el-table-column prop="status" label="状态" sortable width="80px" />
            <el-table-column
              prop="position"
              label="职务"
              sortable
              width="80px"
            />
            <el-table-column align="right" width="120px">
              <template #default="scope">
                <el-button
                  size="small"
                  type="primary"
                  @click="changeEmploy(scope.$index, scope.row)"
                  >进行人事调动</el-button
                >
              </template>
            </el-table-column>
          </el-table>
          <!-- 错误提示 -->
          <div v-else>{{ boxContent }}</div>
        </el-tab-pane>

        <el-tab-pane
          v-if="process === '选择调动警局'"
          label="查询警局"
          name="1"
        >
          <div style="text-align: center">
            <table style="display: inline-block; text-align: left">
              <tr>
                <td>
                  <el-text class="noteText" type="primary">警局ID:</el-text>
                  <el-input
                    class="inputBox"
                    v-model="stationID"
                    placeholder="请输入警局编号"
                    clearable
                    maxlength="9"
                    oninput="value=value.replace(/[^\d.]/g,'')"
                    show-word-limit
                  />
                </td>
                <td>
                  <el-text class="noteText" type="primary">警局名称:</el-text>
                  <el-input
                    class="inputBox"
                    v-model="stationName"
                    placeholder="请输入警局名称"
                    clearable
                  />
                </td>
              </tr>
              <tr>
                <td>
                  <el-text class="noteText" type="primary">所在地址:</el-text>
                  <el-input
                    class="inputBox"
                    v-model="stationAddress"
                    placeholder="请输入地址"
                    clearable
                  />
                </td>
                <td></td>
              </tr>
            </table>
            <!-- 输入结束，下面是按钮 -->
            <div class="seaButton">
              <el-button type="primary" @click="fetchStationInfo"
                >查询</el-button
              >
            </div>
          </div>
          <el-divider />

          <el-table
            v-if="stationInfos.length > 0"
            :data="stationInfos"
            stripe
            height="450"
            @wheel.passive.stop
          >
            <el-table-column prop="stationID" label="警局编号" sortable />
            <el-table-column prop="stationName" label="警局名称" sortable />
            <el-table-column prop="address" label="所在地址" sortable />
            <el-table-column prop="budget" label="预算" sortable />
            <el-table-column align="right" width="260px">
              <template #header>
                <table>
                  <tr>
                    <td>
                      <el-text
                        type="primary"
                        style="display: inline-block; width: 170px"
                        >当前警员警号：{{ selectPolicemenID }}</el-text
                      >
                    </td>
                    <td>
                      <el-button size="small" type="primary" @click="goBack"
                        >重选警员</el-button
                      >
                    </td>
                  </tr>
                </table>
              </template>
              <template #default="scope">
                <el-button
                  size="small"
                  type="primary"
                  @click="finish(scope.$index, scope.row)"
                  >调入此局</el-button
                >
              </template>
            </el-table-column>
          </el-table>
          <div v-else>{{ boxContent }}</div>
        </el-tab-pane>

        <div v-if="process === '确认信息'" style="text-align: center">
          <!--直接展示当前雇佣信息并输入薪水-->
          <div style="text-align: center">
            <table style="display: inline-block; text-align: center">
              <tr>
                <td><span class="ssqtitletest">当前调度信息</span></td>
              </tr>
              <tr>
                <td>
                  <span class="ssqtitletest"
                    >调度警员警号：{{ selectPolicemenID }}</span
                  >
                </td>
              </tr>
              <tr>
                <td>
                  <span class="ssqtitletest"
                    >调入警局警号：{{ selectStationID }}</span
                  >
                </td>
              </tr>

              <tr>
                <td>
                  <el-text class="noteText" type="primary"
                    >输入雇佣薪资:</el-text
                  >
                  <el-input
                    class="inputBox"
                    v-model="salary"
                    placeholder="警员薪水"
                    clearable
                  />
                </td>
              </tr>
              <div class="seaButton">
                <el-button type="primary" @click="employ">确认薪水</el-button>
              </div>
              <div class="seaButton">
                <el-button type="primary" @click="goBack">返回</el-button>
              </div>
            </table>
          </div>
        </div>

        <div v-if="process === '完成'" style="text-align: center">
          <el-result
            icon="success"
            :title="'人事调动完成，任职开始时间为' + this.employInfo.employTime"
            sub-title="点击按钮返回警员查询列表"
          >
            <template #extra>
              <el-button type="primary" @click="goBack">返回</el-button>
            </template>
          </el-result>
        </div>

        <div v-if="process === '失败'" style="text-align: center">
          <el-result
            icon="fail"
            :title="
              '人事调动失败，薪资超过当局预算' +
              this.employInfo.failReason +
              '元'
            "
            sub-title="点击按钮返回警员查询列表"
          >
            <template #extra>
              <el-button type="primary" @click="goBack">返回</el-button>
            </template>
          </el-result>
        </div>
      </el-tabs>
    </section>
  </main>
</template>

<script>
import axios from "../../api/request";

export default {
  data() {
    return {
      anv: "1",
      policemenNumber: "",
      policemenName: "",
      policemenIDNum: "",
      policemenYear: "",
      policemenSex: "",
      policemenNation: "",
      policemenStatus: "",
      policemenPosition: "",
      policemenInfos: [],
      selectPolicemenID: "",

      stationAddress: "",
      stationName: "",
      stationID: "",
      stationInfos: [],
      selectStationID: "",
      salary: 0,

      err: "警员不存在！",
      boxContent: "",
      process: "选择调度警员",
      failreason: "",

      employInfo: [],
    };
  },
  methods: {
    handleMouseMove(event) {
      const x = event.pageX - event.target.offsetLeft;
      const y = event.pageY - event.target.offsetTop;

      event.target.style.setProperty("--x", `${x}px`);
      event.target.style.setProperty("--y", `${y}px`);
    },
    async fetchpolicemenInfo() {
      try {
        const response = await axios.get(
          `/api/policemenInfo?policemenID=${encodeURIComponent(
            this.policemenNumber
          )}&policemenName=${encodeURIComponent(
            this.policemenName
          )}&policemenIDNum=${encodeURIComponent(
            this.policemenIDNum
          )}&policemenYear=${encodeURIComponent(
            this.policemenYear
          )}&policemenSex=${encodeURIComponent(
            this.policemenSex
          )}&policemenNation=${encodeURIComponent(
            this.policemenNation
          )}&policemenStatus=${encodeURIComponent(
            this.policemenStatus
          )}&policemenPosition=${encodeURIComponent(this.policemenPosition)}`
        );
        this.policemenInfos = response.data.map((item) => {
          if (item.gender === "M") {
            return { ...item, gender: "男" };
          } else if (item.gender === "F") {
            return { ...item, gender: "女" };
          } else {
            return item;
          }
        });
      } catch (error) {
        // 请求失败时的处理逻辑
        ElMessage({
          showClose: true,
          message: "获取警员列表失败！",
          type: "error",
          duration: 5000,
        });
      }
    },
    async fetchStationInfo() {
      try {
        const response = await axios.get(
          `/api/stationInfo?stationID=${encodeURIComponent(
            this.stationID
          )}&stationName=${encodeURIComponent(
            this.stationName
          )}&stationAddress=${encodeURIComponent(this.stationAddress)}`
        );
        this.stationInfos = response.data;
      } catch (error) {
        // 请求失败时的处理逻辑
        ElMessage({
          showClose: true,
          message: "获取警局列表失败！",
          type: "error",
          duration: 5000,
        });
      }
    },
    async changeEmploy(index, policemenInfo) {
      this.process = "选择调动警局";
      this.selectPolicemenID = policemenInfo["pid"];
      console.log(this.selectPolicemenID);
      this.err = "警局不存在！";
    },
    async goBack() {
      this.process = "选择调度警员";
      this.selectPolicemenID = "";
      this.err = "警员不存在！";
    },
    async finish(index, StationInfo) {
      this.process = "确认信息";
      this.selectStationID = StationInfo["stationID"];
      console.log(this.selectStationID);
      this.err = "确认信息界面出错";
    },
    async employ() {
      console.log(this.salary);
      if (this.salary <= 0) {
        alert("雇佣薪资需大于〇！");
        return;
      }

      axios
        .post("/api/addEmploy", {
          selectPolicemenID: this.selectPolicemenID,
          selectStationID: this.selectStationID,
          salary: this.salary,
        })
        .then((res) => {
          this.employInfo = res.data;
          console.log(res.data);
          if (res.data.failReason === "success") {
            this.process = "完成";
          } else {
            this.process = "失败";
          }
        })
        .catch((err) => {
          this.boxContent = this.err;
          console.log(err);
        });
    },
  },
};
</script>

<style lang="postcss" scoped>
.ssqtitletest {
  margin: 10px auto;
  min-width: 800px;
  height: 40px;
  padding: 0 30px;
  line-height: 60px;
  text-align: center;
  position: relative;
  appearance: none;
  background: #efe7d6;
  box-shadow: inset -500px 0px 100px 0px rgba(200, 183, 144, 0.838);
  border: none;
  color: white;
  font-size: 25px;
  outline: none;
  /* overflow: hidden; */
  border-radius: 0px;
}
.ssqtitletest span {
  position: relative;
  top: -20%;
}
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
.main {
  margin-top: 10vh;
  justify-content: center;
  align-content: center;
  width: 100%;
  height: 120vh;
  min-width: 800px;
}

.ssqinputinfobox {
  position: relative;
  width: 10vw;
  display: inline-block;
}

.ssqinputtext {
  text-align: center;
  margin-top: 7vh;
  margin-left: 5vw;
  margin-right: 2vw;
  width: 15vw;
  display: inline-block;
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
