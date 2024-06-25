<template>
  <div
    class="main"
    style="width: 60%; display: inline-block; margin-left: 20px"
  >
    <div
      style="
        height: 200px;
        font: 400 25px Inter, sans-serif;
        vertical-align: top;
      "
    >
      <div><br />欢迎回来，{{ myPoliceNumber }}<br /></div>
      <div style="font: 400 20px Inter, sans-serif"><br />开始愉快的一天</div>
    </div>
    <div class="syslogtitle">
      <div class="syslog">系统日志</div>
    </div>

    <el-table
      v-if="auditInfo.length > 0"
      :data="auditInfo"
      stripe
      height="450"
      @wheel.passive.stop
    >
      <el-table-column prop="oS_USERNAME" label="操作系统用户名" width="150" />
      <el-table-column prop="username" label="用户名" width="120" />
      <el-table-column prop="obJ_NAME" label="表名" />
      <el-table-column prop="actioN_NAME" label="操作名称" width="100" />
      <el-table-column prop="actioN_TIME" label="操作时间" />
    </el-table>
  </div>
  <div
    style="
      width: 31%;
      min-height: 100px;
      margin-left: 5%;
      display: inline-block;
      background-color: rgba(255, 255, 255, 0.733);
      vertical-align: top;
    "
  >
    <div class="syslogtitle">
      <div class="syslog">常用功能</div>
      <button
        style="
          height: 80%;
          position: absolute;
          width: 100px;
          right: 10px;
          background-color: #c1b598;
          color: white;
          cursor: pointer;
          border: none;
        "
        @click="showMoreOptions"
      >
        更多功能
      </button>
    </div>

    <div style="min-width: 100%">
      <el-button
        class="ssqfuncbutton"
        v-for="option in selectedOptions"
        :key="option"
        :label="option"
        @click="goTo(option)"
        >{{ option }}</el-button
      >
    </div>
  </div>
  <more-options-dialog
    @options-selected="handleOptionsSelected"
    ref="moreOptionsDialog"
  ></more-options-dialog>
  <el-calendar class="calendar"></el-calendar>
</template>

<script>
import axios from "../../api/request";

import { ref } from "vue";
import MoreOptionsDialog from "@/components/Menu/MoreOptionsDialog.vue";

export default {
  setup() {
    const containerHeight = ref(1500); // 初始化容器高度
    const minHeight = ref(1500); // 最小高度
    const maxScrollHeight = ref(3000); // 最大滚动高度，控制界面的延伸

    const handleScroll = (event) => {
      if (event.deltaY > 0) {
        // 向下滚动
        if (containerHeight.value < maxScrollHeight.value) {
          containerHeight.value += 50;
        }
      } else {
        // 向上滚动
        if (containerHeight.value > minHeight.value) {
          containerHeight.value -= 50;
        }
      }
    };

    return {
      handleScroll,
      containerHeight,
      minHeight,
      maxScrollHeight,
    };
  },
  data() {
    return {
      selectedOptions: ["警员信息管理"],
      myAuthority: localStorage.getItem("authority"),
      myPoliceNumber: localStorage.getItem("policeNumber"),
      dialogVisible: false,

      auditInfo: [],
    };
  },
  components: {
    MoreOptionsDialog,
  },
  methods: {
    goTo(funcName) {
      if (funcName == "注册") {
        this.$router.push("/mainMenu/Register");
      } else if (funcName == "警员权限修改") {
        this.$router.push("/mainMenu/ChangePermission");
      } else if (funcName == "警员考核管理") {
        this.$router.push("/mainMenu/PoliceAssessment");
      } else if (funcName == "警局信息管理") {
        this.$router.push("/mainMenu/PoliceStationInfoManagement");
      } else if (funcName == "警员信息管理") {
        this.$router.push("/mainMenu/PoliceOfficerInfoManagement");
      } else if (funcName == "警员出勤管理") {
        this.$router.push("/mainMenu/AttendanceManagement");
      } else if (funcName == "执法录像管理") {
        this.$router.push("/mainMenu/VideoManagement");
      } else if (funcName == "接警记录管理") {
        this.$router.push("/mainMenu/AlarmResponseRecordManagement");
      } else if (funcName == "薪水管理") {
        this.$router.push("/mainMenu/PayrollManagement");
      } else if (funcName == "嫌疑人信息管理") {
        this.$router.push("/mainMenu/SuspectInfoManagement");
      } else if (funcName == "犯罪记录管理") {
        this.$router.push("/mainMenu/CriminalRecordManagement");
      } else if (funcName == "受害人信息管理") {
        this.$router.push("/mainMenu/VictimInfoManagement");
      } else if (funcName == "案件分类与管理") {
        this.$router.push("/mainMenu/CaseClassificationManagement");
      } else if (funcName == "犯罪数据统计") {
        this.$router.push("/mainMenu/CrimeDataStatistics");
      } else if (funcName == "重点人员统计") {
        this.$router.push("/mainMenu/KeyIndividualsManagement");
      } else if (funcName == "警车管理") {
        this.$router.push("/mainMenu/PoliceVehicleManagement");
      } else if (funcName == "警械管理") {
        this.$router.push("/mainMenu/PoliceEquipmentManagement");
      } else if (funcName == "公民户籍管理") {
        this.$router.push("/mainMenu/CitizenInfoManagement");
      } else if (funcName == "城市地区管理") {
        this.$router.push("/mainMenu/UrbanZoningManagement");
      } else if (funcName == "城市调度管理") {
        this.$router.push("/mainMenu/RegionalDispatch");
      } else if (funcName == "家族背景调查") {
        this.$router.push("/mainMenu/FamilybgCheck");
      } else if (funcName == "案件办理") {
        this.$router.push("/mainMenu/CaseInvestigation");
      }
    },
    showMoreOptions() {
      this.$refs.moreOptionsDialog.showMoreOptions(
        this.selectedOptions,
        this.myAuthority
      );
    },
    handleOptionsSelected(selectedOptions) {
      // 处理选项确认后的逻辑
      // 更新按钮的显示状态等
      this.selectedOptions = selectedOptions;
      localStorage.setItem(
        "selectedOptions_" + this.myPoliceNumber,
        JSON.stringify(selectedOptions)
      );
    },
    handleMouseMove(event) {
      const x = event.pageX - event.target.offsetLeft;
      const y = event.pageY - event.target.offsetTop;
      event.target.style.setProperty("--x", `${x}px`);
      event.target.style.setProperty("--y", `${y}px`);
    },
  },
  created() {
    setTimeout(() => {
      if (location.href.indexOf("?reload=true") < 0) {
        location.href += "?reload=true";
        location.reload();
      }
    }, 100); //2023.9.4 每次进入页面时刷新一次

    const myPoliceNumber = localStorage.getItem("policeNumber");
    const savedOptions = localStorage.getItem(
      "selectedOptions_" + myPoliceNumber
    );
    if (savedOptions) {
      console.log(savedOptions);
      this.selectedOptions = JSON.parse(savedOptions);
    }

    axios
      .post(`/api/audit`, {
        myAuthority: localStorage.getItem("authority"),
        myPoliceNumber: localStorage.getItem("policeNumber"),
      })
      .then((response) => {
        this.auditInfo = response.data;
      })
      .catch((error) => {
        // 请求失败时的处理逻辑
        alert(error);
        console.log("函数出错：" + error);
      });
  },
};
</script>

<style scoped>
.syslogtitle {
  width: 100%;
  height: 30px;
  position: relative;
  overflow: hidden;
  display: flex;
  align-items: center;
  padding-left: 20px;
  background: #d7caaa;
}
.syslog {
  color: #ffffff;
  text-align: left;
  display: relative;
  left: 80px;
  width: 100%;
  font: 400 20px "Inter", sans-serif;
  padding-left: 0px;
}
.ssqfuncbutton {
  margin-top: 5px;
  margin-left: 28px;
  width: 100px;
}
.calendar {
  display: float;
  position: absolute;
  top: 40vh; /* 顶部距离为0，即页面上边缘 */
  right: 1vw; /* 右侧距离为0，即页面右边缘 */
  overflow: auto;
  width: 31%;
}
</style>
