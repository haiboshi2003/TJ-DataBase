<template>
  <div>
    <input v-model="policeNumber" placeholder="输入警号" />
    <button @click="searchPolice">搜索</button>
    <div v-if="policeInfo">
      <p>警员姓名：{{ policeInfo.police_name }}</p>
      <p>当前职位：{{ policeInfo.position }}</p>
      <select v-model="selectedPosition">
        <option value="学员">学员</option>
        <option value="警员">警员</option>
        <option value="警督">警督</option>
        <option value="警司">警司</option>
        <option value="警监">警监</option>
        <option value="总警监">总警监</option>
      </select>
      <input v-model="reason" placeholder="输入修改原因" />
      <button @click="updatePosition">确认修改</button>
      <div v-if="updateSuccess" class="success-message">修改成功</div>
      <div v-else-if="updateFailed" class="error-message">申请失败</div>
    </div>
    <div v-else>
      <p>未找到警员信息</p>
    </div>
  </div>
</template>

<script>
import axios from "../../api/request";

export default {
  data() {
    return {
      policeNumber: "",
      policeInfo: null,
      selectedPosition: "",
      reason: "",
      updateSuccess: false,
      updateFailed: false,
    };
  },
  methods: {
    searchPolice() {
      this.policeInfo = axios
        .post("/api/search", { temp: this.policeNumber })
        .then((response) => {
          this.policeInfo = response.data;
          //   this.updateSuccess = false
          //   this.updateFailed = false
        })
        .catch((error) => {
          console.error(error);
          this.policeInfo = null;
        });
    },
    async updatePosition() {
      // 模拟更新职位
      const isSuccess = await this.updatePositionInDatabase(
        this.policeInfo,
        this.selectedPosition,
        this.reason
      );
      if (isSuccess) {
        this.updateSuccess = true;
        this.updateFailed = false;
        this.searchPolice();
      } else {
        this.updateSuccess = false;
        this.updateFailed = true;
      }
    },
    updatePositionInDatabase(policeInfo, newPosition, reason) {
      return new Promise((resolve, reject) => {
        axios
          .post("/api/position", {
            police_number: policeInfo.police_number,
            police_name: policeInfo.police_name,
            position: newPosition,
            ID_number: policeInfo.ID_number,
            phone_number: policeInfo.phone_number,
            gender: policeInfo.gender,
          })
          .then((response) => {
            this.policeInfo = response.data;
            resolve(true);
          })
          .catch((error) => {
            console.error(error);
            resolve(false);
          });
      });
    },
  },
};
</script>

<style scoped>
.success-message {
  color: green;
  margin-top: 10px;
}

.error-message {
  color: red;
  margin-top: 10px;
}
</style>
