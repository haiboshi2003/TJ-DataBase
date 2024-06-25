<template>
  <div>
    <form @submit.prevent="submitForm">
      <div class="form-group">
        <label for="id">ID:</label>
        <input type="text" id="id" v-model="employeeData.police_number_receive" required>
      </div>
      <div class="form-group">
        <label for="basicSalary">基本工资:</label>
        <input type="number" id="basicSalary" v-model="employeeData.basic_amount" required>
      </div>
      <div class="form-group">
        <label for="bonus">奖金:</label>
        <input type="number" id="bonus" v-model="employeeData.reward_amount" required>
      </div>
      <div class="form-group">
        <label for="description">描述:</label>
        <textarea id="description" v-model="employeeData.description"></textarea>
      </div>
      <button type="submit">确认</button>
    </form>
  </div>
</template>

<script>
import axios from 'axios';

export default {
  data() {
    return {
      employeeData: {
        police_number_receive: '',
        basic_amount: '',
        reward_amount: '',
        description: ''
      }
    };
  },
  
  methods: {
    submitForm() {
      // 从localStorage中获取Token
      const token = localStorage.getItem("token");

      // 设置Axios请求的headers，包括Authorization头部
      const headers = {
        Authorization: `Bearer ${token}`
      };

      // 使用axios发送数据到后端，并在headers中包含Token
      axios.post('http://localhost:7078/salary_newRecord', this.employeeData, { headers })
        .then(response => {
          // 处理响应
          console.log(response.data);
        })
        .catch(error => {
          // 处理错误
          console.error(error);
        });
    }
  }
}
</script>

<style scoped>
/* 样式可以根据你的需求进行自定义 */
.form-group {
  margin-bottom: 15px;
}
label {
  font-weight: bold;
}
textarea {
  height: 100px;
}
button {
  background-color: #007bff;
  color: #fff;
  padding: 10px 20px;
  border: none;
  cursor: pointer;
}
button:hover {
  background-color: #0056b3;
}
</style>
