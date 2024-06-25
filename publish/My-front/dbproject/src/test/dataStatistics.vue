<template>
  <div>
    <form @submit.prevent="search">
      <label for="police_number">警号:</label>
      <input type="text" police_number="police_number" v-model="searchData.police_number" />

      <label for="name">姓名:</label>
      <input type="text" id="name" v-model="searchData.name" />

      <label for="year">年份:</label>
      <select id="year" v-model="searchData.year">
        <option value="">全部</option>
        <option v-for="year in years" :value="year">{{ year }}</option>
      </select>

      <label for="month">月份:</label>
      <select id="month" v-model="searchData.month">
        <option value="">全部</option>
        <option v-for="month in months" :value="month.value">{{ month.label }}</option>
      </select>

      <label for="station">警局:</label>
      <select id="station" v-model="searchData.station">
        <option value="">全部</option>
        
        <option value="320103014">徐州市公安局鼓楼分局</option>
        // 这里加 警局编号 以及 名字
      </select>

      <button type="submit">搜索</button>
    </form>

    <div v-if="showResults">
      <h2>搜索结果</h2>
      <table>
        <thead>
          <tr>
            <th>PAYROLL_NUMBER</th>
            <th>POLICE_NUMBER</th>
            <th>POLICE_NAME</th>
            <th>STATION_ID</th>
            <th>PAY_DAY</th>
            <th>SALARY</th>
            <th>SUBSIDY</th>
            <th>DESCRIPTION</th>
            <th>ISSUE_ID</th>
            <th>ISSUE_NAME</th>
          </tr>
        </thead>
        <tbody>
          <!-- 使用 v-for 循环显示搜索结果 -->
          <tr v-for="result in searchResults" :key="result.PAYROLL_NUMBER">
            <td>{{ result.PAYROLL_NUMBER }}</td>
            <td>{{ result.POLICE_NUMBER }}</td>
            <td>{{ result.POLICE_NAME }}</td>
            <td>{{ result.STATION_ID }}</td>
            <td>{{ result.PAY_DAY }}</td>
            <td>{{ result.SALARY }}</td>
            <td>{{ result.SUBSIDY }}</td>
            <td>{{ result.DESCRIPTION }}</td>
            <td>{{ result.ISSUE_ID }}</td>
            <td>{{ result.ISSUE_NAME }}</td>
          </tr>
        </tbody>
      </table>
    </div>

  </div>
</template>

<script>
import axios from 'axios';

export default {
  data() {
    return {
      searchData: {
        police_number: '',
        name: '',
        year: '', // 存储年份
        month: '', // 存储月份
        station: '',
        showResults: false, // 控制是否显示搜索结果
        searchResults: []   // 存储搜索结果
      },
      years: ['2023', '2022', '2021'], // 年份选项
      months: [
        { value: '01', label: '一月' },
        { value: '02', label: '二月' },
        { value: '03', label: '三月' },
        { value: '04', label: '四月' },
        { value: '05', label: '五月' },
        { value: '06', label: '六月' },
        { value: '07', label: '七月' },
        { value: '08', label: '八月' },
        { value: '09', label: '九月' },
        { value: '10', label: '十月' },
        { value: '11', label: '十一月' },
        { value: '12', label: '十二月' },
        // ... 还可以继续添加其他月份
      ]
    };
  },
  methods: {
    search() {
      // 在这里执行搜索操作
      // 检查 searchData 中至少有一个字段非空
      if (!this.searchData.police_number && !this.searchData.name && !this.searchData.year && !this.searchData.month && !this.searchData.station) {
        alert('搜索条件不足！');
        return;
      }

      // 这里可以发送搜索请求，使用 searchData 中的值来过滤数据
      const token = localStorage.getItem("token");

      // 设置Axios请求的headers，包括Authorization头部
      const headers = {
        Authorization: `Bearer ${token}`
      };

      // 使用axios发送数据到后端，并在headers中包含Token
      axios.post('http://localhost:7078/searchWagesRecord', this.searchData, {headers})
        .then(response => {
        // 处理后端返回的数据
        
        this.searchResults = response.data.map(result => {
          let newResult = {};
          for (let key in result) {
            newResult[key.toUpperCase()] = result[key];
          }
          return newResult;
        });

        // 显示搜索结果区域
        this.showResults = true;
      })
      .catch(error => {
        // 处理请求错误
        console.error('请求错误:', error);        // 隐藏搜索结果区域
        this.showResults = false;
        // 显示错误消息
        alert('请求错误，请稍后重试');
      });
  }
 }
}

</script>

