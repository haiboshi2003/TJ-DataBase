<template>
  <el-header class="sub-header" @mousemove="handleMouseMove">
    <div>&nbsp;&nbsp;警员管理&nbsp;>&nbsp;修改申请管理</div>
  </el-header>

  <div class="container">
    <div class="content" @wheel.passive.stop>
      <!-- 权限申请列表 -->
      <div class="ssqtitletest">
        <span>权&nbsp;限&nbsp;申&nbsp;请&nbsp;列&nbsp;表</span>
      </div>
      <table class="custom-table">
        <thead>
          <tr>
            <th style="width: 20px">
              <input
                type="checkbox"
                v-model="selectAll"
                @change="toggleSelectAll"
              />
            </th>
            <th style="width: 80px">申请人</th>
            <th style="width: 100px">被修改人</th>
            <th style="width: 100px">原权限等级</th>
            <th style="width: 120px">修改权限等级</th>
            <th style="width: 80px">申请状态</th>
            <th style="width: 200px">申请原因</th>
            <th style="width: 80px">操作</th>
          </tr>
        </thead>
        <tbody>
          <tr
            v-for="request in pendingRequests"
            :key="
              request.h_number +
              '-' +
              request.s_number +
              '-' +
              request.F_level +
              '-' +
              request.L_level
            "
          >
            <td>
              <input
                type="checkbox"
                v-model="request.selected"
                v-if="request.status === '待处理'"
              />
            </td>
            <td>{{ request.h_number }}</td>
            <td>{{ request.s_number }}</td>
            <td>{{ request.f_level }}</td>
            <td>{{ request.l_level }}</td>
            <td>{{ request.status }}</td>
            <td class="wrap-cell">{{ request.reason }}</td>
            <td>
              <button
                class="cpbutton"
                style="margin-top: -5px"
                @click="approveRequest(request)"
                v-if="request.status === '待处理'"
              >
                <span>同意</span>
              </button>
              <button
                class="cpbutton"
                @click="rejectRequest(request)"
                v-if="request.status === '待处理'"
              >
                <span>拒绝</span>
              </button>
            </td>
          </tr>
        </tbody>
      </table>
      <!-- 批量审批操作区域 -->
      <div>
        <button class="cpbutton1" @click="batchApprove">
          <span>批量同意</span>
        </button>
        <button
          class="cpbutton1"
          style="margin-left: 40px"
          @click="batchReject"
        >
          <span>批量拒绝</span>
        </button>
      </div>

      <!-- 已同意的申请 -->
      <div class="ssqtitletest" style="margin-top: 80px">
        <span>已&nbsp;同&nbsp;意&nbsp;的&nbsp;申&nbsp;请</span>
      </div>
      <table class="custom-table">
        <!-- 表头 -->
        <thead>
          <tr>
            <th>申请人警号</th>
            <th>被修改人警号</th>
            <th>原权限等级</th>
            <th>修改权限等级</th>
            <th>申请状态</th>
            <th>申请原因</th>
          </tr>
        </thead>
        <!-- 表格内容 -->
        <tbody>
          <tr
            v-for="request in approvedRequests"
            :key="
              request.h_number +
              '-' +
              request.s_number +
              '-' +
              request.F_level +
              '-' +
              request.L_level
            "
          >
            <td>{{ request.h_number }}</td>
            <td>{{ request.s_number }}</td>
            <td>{{ request.f_level }}</td>
            <td>{{ request.l_level }}</td>
            <td>{{ request.status }}</td>
            <td>{{ request.reason }}</td>
          </tr>
        </tbody>
      </table>

      <!-- 已拒绝的申请 -->
      <div class="ssqtitletest" style="margin-top: 50px">
        <span>已&nbsp;拒&nbsp;绝&nbsp;的&nbsp;申&nbsp;请</span>
      </div>
      <table class="custom-table">
        <!-- 表头 -->
        <thead>
          <tr>
            <th>申请人警号</th>
            <th>被修改人警号</th>
            <th>原权限等级</th>
            <th>修改权限等级</th>
            <th>申请状态</th>
            <th>申请原因</th>
          </tr>
        </thead>
        <!-- 表格内容 -->
        <tbody>
          <tr
            v-for="request in rejectedRequests"
            :key="
              request.h_number +
              '-' +
              request.s_number +
              '-' +
              request.F_level +
              '-' +
              request.L_level
            "
          >
            <td>{{ request.h_number }}</td>
            <td>{{ request.s_number }}</td>
            <td>{{ request.f_level }}</td>
            <td>{{ request.l_level }}</td>
            <td>{{ request.status }}</td>
            <td>{{ request.reason }}</td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>

<script>
import axios from "../../api/request";

export default {
  data() {
    return {
      pendingRequests: [], // 将requests改为pendingRequests
      approvedRequests: [],
      rejectedRequests: [],
      selectAll: false,
    };
  },
  mounted() {
    this.fetchRequests();
  },
  methods: {
    handleMouseMove(event) {
      const x = event.pageX - event.target.offsetLeft;
      const y = event.pageY - event.target.offsetTop;

      event.target.style.setProperty("--x", `${x}px`);
      event.target.style.setProperty("--y", `${y}px`);
    },
    toggleSelectAll() {
      this.pendingRequests.forEach((request) => {
        request.selected = this.selectAll;
      });
    },
    fetchRequests() {
      axios
        .get("/api/manage")
        .then((response) => {
          const pendingRequests = []; // 存储待处理的申请
          const approvedRequests = []; // 存储已同意的申请
          const rejectedRequests = []; // 存储已拒绝的申请
          response.data.forEach((request) => {
            if (request.status === "待处理") {
              pendingRequests.push(request);
            } else if (request.status === "同意") {
              approvedRequests.push(request);
            } else if (request.status === "拒绝") {
              rejectedRequests.push(request);
            }
          });
          this.pendingRequests = pendingRequests;
          this.approvedRequests = approvedRequests;
          this.rejectedRequests = rejectedRequests;
        })
        .catch((error) => {
          console.error(error);
        });
    },
    approveRequest(request) {
      axios
        .post("/api/manage", {
          s_number: request.s_number,
          h_number: request.h_number,
          F_level: request.f_level,
          L_level: request.l_level,
          reason: request.reason,
          status: "同意",
        })
        .then((response) => {
          this.fetchRequests();
        })
        .catch((error) => {
          console.error(error);
        });
    },
    rejectRequest(request) {
      axios
        .post("/api/manage", {
          s_number: request.s_number,
          h_number: request.h_number,
          F_level: request.f_level,
          L_level: request.l_level,
          reason: request.reason,
          status: "拒绝",
        })
        .then((response) => {
          this.fetchRequests();
        })
        .catch((error) => {
          console.error(error);
        });
    },
    batchApprove() {
      const selectedRequests = this.pendingRequests.filter(
        (request) => request.selected
      );
      selectedRequests.forEach((request) => {
        this.approveRequest(request);
      });
    },
    batchReject() {
      const selectedRequests = this.pendingRequests.filter(
        (request) => request.selected
      );
      selectedRequests.forEach((request) => {
        this.rejectRequest(request);
      });
    },
  },
};
</script>

<style scoped>
.wrap-cell {
  width: 40px;
  white-space: pre-wrap;
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
.container {
  margin-top: 10vh;
  min-width: 700px;
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
}
.content {
  width: 50%;
  min-width: 1000px;
  max-height: 300vh;
  overflow: auto;

  box-shadow: 0px 0px 10px 2px rgba(123, 103, 75, 0.427);
  background-color: rgba(255, 255, 255, 0.616);
  margin-bottom: 30px;
  display: flex;
  flex-direction: column;
  justify-content: flex-start;
  margin-top: 16px;
  padding: 16px;

  border-radius: 5px;
  box-shadow: #9a9a9a 0px 0px 6px;
  box-shadow: #777777 0px 0px 3px;
  border-top: #0051ff 3px solid;
  border-top: solid 3px transparent;
}

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

.custom-table {
  border-collapse: collapse;
  margin-top: 5px;
  margin-left: 80px;
  width: 800px;
  position: relative;
  border: 1px solid #ccc;
  box-shadow: 0px 5px 5px 0px #dbd1bb90;
}

.custom-table th,
.custom-table td {
  border: 1px solid rgb(224, 224, 224);
  padding: 8px;
  height: 50px;
  text-align: center;
  position: relative;
}

.custom-table th {
  background-color: rgb(255, 255, 255);

  color: #94a6cd;
}
.custom-table td {
  background-color: rgb(255, 255, 255);
  color: #585e6d;
}

.cpbutton {
  margin-left: 5px;
  margin-top: 5px;
  bottom: -6px;
  width: 50px;
  height: 30px;
  padding: 0 10px;
  line-height: 40px;
  text-align: center;
  position: relative;
  appearance: none;
  background: #5761ee;
  border: none;
  color: white;
  font-size: 15px;
  cursor: pointer;
  outline: none;
  overflow: hidden;
  border-radius: 6px;
  display: block;
}

.cpbutton span {
  position: relative;
  top: -20%;
}

.cpbutton::before {
  --size: 0;
  content: "";
  position: absolute;
  left: var(--x);
  top: var(--y);
  width: var(--size);
  height: var(--size);
  background: radial-gradient(circle closest-side, #cdc1a9, transparent);
  transform: translate(-50%, -50%);
  transition: width 0.2s ease, height 0.2s ease;
}

.cpbutton:hover::before {
  --size: 400px;
}

.cpbutton1 {
  margin-left: 80px;
  margin-top: 5px;
  bottom: -6px;
  width: 200px;
  height: 35px;
  padding: 0 10px;
  line-height: 45px;
  text-align: center;
  position: relative;
  appearance: none;
  background: #e2c895;
  border: none;
  color: white;
  font-size: 20px;
  cursor: pointer;
  outline: none;
  overflow: hidden;
  border-radius: 100px;
  display: inline-block;
}

.cpbutton1 span {
  position: relative;
  top: -20%;
}

.cpbutton1::before {
  --size: 0;
  content: "";
  position: absolute;
  left: var(--x);
  top: var(--y);
  width: var(--size);
  height: var(--size);
  background: radial-gradient(circle closest-side, #5761ee, transparent);
  transform: translate(-50%, -50%);
  transition: width 0.2s ease, height 0.2s ease;
}

.cpbutton1:hover::before {
  --size: 400px;
}
</style>
