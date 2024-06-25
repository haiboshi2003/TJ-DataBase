<!-- zyh -->
<template>
  <el-header class="sub-header" @mousemove="handleMouseMove">
    <div>&nbsp;&nbsp;案件管理&nbsp;>&nbsp;犯罪数据统计</div>
  </el-header>

  <div class="main">
    <div class="content">
      <el-tabs v-model="anv">
        <el-tab-pane label="案件状态统计饼图" name="1">
          <div class="top-content">
            <h2 style="text-align: center">案件状态统计饼图</h2>
            <div id="statusPieChart" style="width: 400px; height: 400px; margin-left: auto; margin-right: auto;"></div>
          </div>
          <div class="bottom-content">
            <div class="cdsamount">
              <h3>案件总数：{{ numCases }}</h3>
            </div>
            <select v-model="selectedCity1" class="cds-select">
              <option value="全部" selected>全部城市</option>
              <option v-for="city in cityName" :value="city" :key="city">{{ city }}</option>
            </select>
            <select v-model="selectedYear1" class="cds-select">
              <option selected value="全部">全部年份</option>
              <option v-for="i in years" :value="i" :key="i">{{ i }}</option>
            </select>
            <select v-model="selectedMonth1" class="cds-select">
              <option selected value="全部">全部月份</option>
              <option v-for="i in ['1', '2', '3', '4', '5', '6', '7', '8', '9', '10', '11', '12']" :value="i" :key="i">{{
                i }}
              </option>
            </select>
          </div>
        </el-tab-pane>

        <el-tab-pane label="案件类型统计饼图" name="2">
          <div class="top-content">
            <h2 style="text-align: center">案件类型统计饼图</h2>
            <div id="typePieChart" style="width: 600px; height: 400px; margin-left: auto; margin-right: auto"></div>
          </div>

          <div class="bottom-content">
            <div class="cdsamount">
              <h3>案件总数：{{ numCases }}</h3>
            </div>
            <select v-model="selectedCity2" class="cds-select">
              <option value="全部" selected>全部城市</option>
              <option v-for="city in cityName" :value="city" :key="city">{{ city }}</option>
            </select>
            <select v-model="selectedYear2" class="cds-select">
              <option selected value="全部">全部年份</option>
              <option v-for="i in years" :value="i" :key="i">{{ i }}</option>
            </select>
            <select v-model="selectedMonth2" class="cds-select">
              <option selected value="全部">全部月份</option>
              <option v-for="i in ['1', '2', '3', '4', '5', '6', '7', '8', '9', '10', '11', '12']" :value="i" :key="i">{{
                i }}
              </option>
            </select>
          </div>
        </el-tab-pane>

        <el-tab-pane label="案发地区统计直方图" name="3">
          <div class="top-content">
            <h2 style="text-align: center">城市案件统计直方图</h2>
            <div id="zhuChart" style="width: 800px; height: 500px; margin-left: auto; margin-right: auto"></div>
          </div>
          <div class="bottom-content">
            <div class="cdsamount" style="margin-top:-25px">
              <h3>案件总数：{{ numCases }}</h3>
            </div>
          </div>
        </el-tab-pane>

        <el-tab-pane label="案件时间统计折线图" name="4">
          <div class="top-content">
            <h2 style="text-align: center">案件时间统计折线图</h2>
            <div id="zheChart" style="width: 600px; height: 400px; margin-left: auto; margin-right: auto"></div>
          </div>
          <div class="bottom-content">
            <div class="cdsamount">
              <h3>案件总数：{{ numCases }}</h3>
            </div>
            <select v-model="selectedCity3" class="cds-select">
              <option value="全部" selected>全部城市</option>
              <option v-for="city in cityName" :value="city" :key="city">{{ city }}</option>
            </select>
            <select v-model="selectedMethod" class="cds-select">
              <option selected value="年份">年份统计</option>
              <option selected value="月份">月份统计</option>
            </select>
            <select v-if="selectedMethod === '月份'" v-model="selectedYear3" class="cds-select">
              <option selected value="全部">全部年份</option>
              <option v-for="i in years" :value="i" :key="i">{{ i }}</option>
            </select>
          </div>
        </el-tab-pane>
      </el-tabs>
    </div>
  </div>
</template>

<script>
import axios from "../../api/request";

import * as echarts from 'echarts';

export default {
  data() {
    return {
      anv: "1",
      numCases: 0,
      statusStatistics: "默认",
      typeStatistics: "默认",
      cityStatistics: "默认",
      cityTypeStatistics: "默认",
      cityYearStatistics: "默认",
      cityName: "默认",
      selectedCity1: "全部",
      selectedYear1: "全部",
      selectedMonth1: "全部",
      selectedCity2: "全部",
      selectedYear2: "全部",
      selectedMonth2: "全部",
      selectedCity3: "全部",
      selectedYear3: "全部",
      cityDateStatistics: "默认",
      years: [],
      selectedMethod: "年份",
    };
  },
  methods: {
    handleMouseMove(event) {
      const x = event.pageX - event.target.offsetLeft;
      const y = event.pageY - event.target.offsetTop;

      event.target.style.setProperty("--x", `${x}px`);
      event.target.style.setProperty("--y", `${y}px`);
    },
    async getCityStatistics() {
      try {
        const response = await axios.get("/api/crimeDataStatistics/caseCityStatistics");
        this.cityStatistics = response.data;
      } catch (error) {
        // 请求失败时的处理逻辑
        ElMessage({
          showClose: true,
          message: "获取统计信息失败！",
          type: 'error',
          duration: 5000,
        });
      }
    },
    async getCityDateStatistics() {
      try {
        const response = await axios.get("/api/crimeDataStatistics/cityDateStatistics", { params: { city: this.selectedCity3 } });
        this.cityDateStatistics = response.data;
      } catch (error) {
        // 请求失败时的处理逻辑
        ElMessage({
          showClose: true,
          message: "获取统计信息失败！",
          type: 'error',
          duration: 5000,
        });
      }
    },
    async getStatusCityDateStatistics() {
      try {
        const response = await axios.get(`/api/crimeDataStatistics/statusCityDateStatistics?city=${encodeURIComponent(this.selectedCity1)}&year=${encodeURIComponent(this.selectedYear1)}&month=${encodeURIComponent(this.selectedMonth1)}`);
        this.statusStatistics = response.data;
      } catch (error) {
        // 请求失败时的处理逻辑
        ElMessage({
          showClose: true,
          message: "获取统计信息失败！",
          type: 'error',
          duration: 5000,
        });
      }
    },
    async getCityTypeStatistics() {
      try {
        const response = await axios.get("/api/crimeDataStatistics/cityTypeStatistics");
        this.cityTypeStatistics = response.data;
      } catch (error) {
        // 请求失败时的处理逻辑
        ElMessage({
          showClose: true,
          message: "获取统计信息失败！",
          type: 'error',
          duration: 5000,
        });
      }
    },
    async getTypeCityDateStatistics() {
      try {
        const response = await axios.get(`/api/crimeDataStatistics/typeCityDateStatistics?city=${encodeURIComponent(this.selectedCity2)}&year=${encodeURIComponent(this.selectedYear2)}&month=${encodeURIComponent(this.selectedMonth2)}`);
        this.typeStatistics = response.data;
      } catch (error) {
        // 请求失败时的处理逻辑
        ElMessage({
          showClose: true,
          message: "获取统计信息失败！",
          type: 'error',
          duration: 5000,
        });
      }
    },
    drawStatusPieChart() {
      const pieChart = echarts.init(document.getElementById('statusPieChart'));

      const option = {
        tooltip: {
          trigger: 'item'
        },
        legend: {
          // orient: 'vertical',
          top: '5%',
          left: 'center'
        },
        series: [
          {
            name: 'Access From',
            type: 'pie',
            radius: ['40%', '70%'],
            avoidLabelOverlap: false,
            itemStyle: {
              borderRadius: 10,
              borderColor: '#fff',
              borderWidth: 2
            },
            label: {
              show: false,
              position: 'center'
            },
            emphasis: {
              label: {
                show: true,
                fontSize: 20,
                fontWeight: 'bold'
              }
            },
            labelLine: {
              show: false
            },
            data: [
              { value: this.statusStatistics['立案'], name: '立案' },
              { value: this.statusStatistics['调查'], name: '调查' },
              { value: this.statusStatistics['结案'], name: '结案' }
            ]
          }
        ]
      };

      option && pieChart.setOption(option);
    },
    drawTypePieChart() {
      const pieChart = echarts.init(document.getElementById('typePieChart'));

      const option = {
        tooltip: {
          trigger: 'item'
        },
        legend: {
          top: '5%',
          left: 'center'
        },
        series: [
          {
            name: 'Access From',
            type: 'pie',
            radius: ['40%', '70%'],
            avoidLabelOverlap: false,
            itemStyle: {
              borderRadius: 10,
              borderColor: '#fff',
              borderWidth: 2
            },
            label: {
              show: false,
              position: 'center'
            },
            emphasis: {
              label: {
                show: true,
                fontSize: 20,
                fontWeight: 'bold'
              }
            },
            labelLine: {
              show: false
            },
            data: [
              { value: this.typeStatistics['强奸'], name: '强奸' },
              { value: this.typeStatistics['抢劫'], name: '抢劫' },
              { value: this.typeStatistics['故意伤害'], name: '故意伤害' },
              { value: this.typeStatistics['盗窃'], name: '盗窃' },
              { value: this.typeStatistics['诈骗'], name: '诈骗' },
              { value: this.typeStatistics['谋杀'], name: '谋杀' }
            ]
          }
        ]
      };

      option && pieChart.setOption(option);
    },
    drawYearZheChart() {
      var result = {}; // 保存结果的对象
      for (const [year, yearData] of Object.entries(this.cityDateStatistics)) {
        let sum = 0;
        for (const value of Object.values(yearData)) {
          sum += value;
        }
        result[year] = sum;
      }
      result = Object.values(result); // result: [6, 6, 7, 4, 5, 4, 78]

      const zheChart = echarts.init(document.getElementById('zheChart'));

      const option = {
        xAxis: {
          type: 'category',
          data: this.years
        },
        yAxis: {
          type: 'value'
        },
        series: [
          {
            data: result,
            type: 'line'
          }
        ]
      };

      option && zheChart.setOption(option);
    },
    drawMonthZheChart() {
      const zheChart = echarts.init(document.getElementById('zheChart'));
      var option;

      if (this.selectedYear3 !== "全部") {

        option = {
          xAxis: {
            type: 'category',
            data: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec']
          },
          yAxis: {
            type: 'value'
          },
          series: [
            {
              data: [this.cityDateStatistics[this.selectedYear3]["1"], this.cityDateStatistics[this.selectedYear3]["2"],
              this.cityDateStatistics[this.selectedYear3]["3"], this.cityDateStatistics[this.selectedYear3]["4"],
              this.cityDateStatistics[this.selectedYear3]["5"], this.cityDateStatistics[this.selectedYear3]["6"],
              this.cityDateStatistics[this.selectedYear3]["7"], this.cityDateStatistics[this.selectedYear3]["8"],
              this.cityDateStatistics[this.selectedYear3]["9"], this.cityDateStatistics[this.selectedYear3]["10"],
              this.cityDateStatistics[this.selectedYear3]["11"], this.cityDateStatistics[this.selectedYear3]["12"]],
              type: 'line'
            }
          ]
        };
      }
      else {
        var result = {}; // 保存结果的对象

        for (const [year, yearData] of Object.entries(this.cityDateStatistics)) {
          year; // 为了取消year未使用的报错
          for (const [month, value] of Object.entries(yearData)) {
            if (!result[month]) {
              result[month] = value;
            } else {
              result[month] += value;
            }
          }
        }
        result = Object.values(result);

        option = {
          xAxis: {
            type: 'category',
            data: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec']
          },
          yAxis: {
            type: 'value'
          },
          series: [
            {
              data: [result["1"], result["2"],
              result["3"], result["4"],
              result["5"], result["6"],
              result["7"], result["8"],
              result["9"], result["10"],
              result["11"], result["12"]],
              type: 'line'
            }
          ]
        };
      }

      option && zheChart.setOption(option);
    },
    drawZheChart() {
      if (this.selectedMethod === "年份") {
        this.drawYearZheChart();
      }
      else {
        this.drawMonthZheChart();
      }
    },
    drawTypeZhuChart() {
      var cityType = {
        numRape: [],
        numRobbery: [],
        numInjury: [],
        numTheft: [],
        numFraud: [],
        numMurder: []
      };
      for (const city in this.cityTypeStatistics) {
        if (Object.prototype.hasOwnProperty.call(this.cityTypeStatistics, city)) {
          const crimeCounts = this.cityTypeStatistics[city];
          // 将对应的值添加到转换后的对象中的数组中
          cityType.numRape.push(crimeCounts[0]);
          cityType.numRobbery.push(crimeCounts[1]);
          cityType.numInjury.push(crimeCounts[2]);
          cityType.numTheft.push(crimeCounts[3]);
          cityType.numFraud.push(crimeCounts[4]);
          cityType.numMurder.push(crimeCounts[5]);
        }
      }

      const values = Object.values(this.cityTypeStatistics);
      const flattenedValues = values.reduce((acc, val) => acc.concat(val), []);
      var max = Math.max(...flattenedValues);

      const zhuChart = echarts.init(document.getElementById('zhuChart'));

      const option = {
        tooltip: {
          trigger: 'axis'
        },
        legend: {
          data: ['强奸', '抢劫', '故意伤害', '盗窃', '诈骗', '谋杀']
        },
        toolbox: {
          show: true,
          feature: {
            dataView: { show: true, readOnly: false },
            magicType: { show: true, type: ['line', 'bar'] },
            restore: { show: true },
            saveAsImage: { show: true }
          }
        },
        calculable: true,
        dataZoom: [
          //1.横向使用滚动条
          {
            type: 'slider',//有单独的滑动条，用户在滑动条上进行缩放或漫游。inside是直接可以是在内部拖动显示
            show: false,//是否显示 组件。如果设置为 false，不会显示，但是数据过滤的功能还存在。
            start: 0,//数据窗口范围的起始百分比0-100
            end: 50,//数据窗口范围的结束百分比0-100
            xAxisIndex: [0],// 此处表示控制第一个xAxis，设置 dataZoom-slider 组件控制的 x轴 可是已数组[0,2]表示控制第一，三个；xAxisIndex: 2 ，表示控制第二个。yAxisIndex属性同理
          },
          //2.在内部可以横向拖动
          {
            type: 'inside',// 内置于坐标系中
            start: 0,
            end: 30,
            xAxisIndex: [0],
            zoomOnMouseWheel: false, // 关闭滚轮缩放
            moveOnMouseWheel: true,
            moveOnMouseMove: false,
          }
        ],
        xAxis: [
          {
            type: 'category',
            // prettier-ignore
            data: Object.keys(this.cityTypeStatistics)
          }
        ],
        yAxis: [
          {
            max: max,
            min: 0,
            type: 'value'
          }
        ],
        series: [
          {
            name: '强奸',
            type: 'bar',
            data: cityType.numRape,
            barWidth: '10%'
            // barCategoryGap: '33%',
            // barGap: '30%',
          },
          {
            name: '抢劫',
            type: 'bar',
            data: cityType.numRobbery,
            barWidth: '10%'
            // barCategoryGap: '33%',
            // barGap: '30%',
          },
          {
            name: '故意伤害',
            type: 'bar',
            data: cityType.numInjury,
            barWidth: '10%'
            // barCategoryGap: '33%',
            // barGap: '30%',
          },
          {
            name: '盗窃',
            type: 'bar',
            data: cityType.numTheft,
            barWidth: '10%'
            // barCategoryGap: '33%',
            // barGap: '30%',
          },
          {
            name: '诈骗',
            type: 'bar',
            data: cityType.numFraud,
            barWidth: '10%'
            // barCategoryGap: '33%',
            // barGap: '30%',
          },
          {
            name: '谋杀',
            type: 'bar',
            data: cityType.numMurder,
            barWidth: '10%'
            // barCategoryGap: '33%',
            // barGap: '30%',
          }
        ]
      };

      option && zhuChart.setOption(option);
    },
  },
  watch: {
    async selectedCity1() {
      await this.getStatusCityDateStatistics();
      this.drawStatusPieChart();
    },
    async selectedYear1() {
      await this.getStatusCityDateStatistics();
      this.drawStatusPieChart();
    },
    async selectedMonth1() {
      await this.getStatusCityDateStatistics();
      this.drawStatusPieChart();
    },
    async selectedCity2() {
      await this.getTypeCityDateStatistics();
      this.drawTypePieChart();
    },
    async selectedYear2() {
      await this.getTypeCityDateStatistics();
      this.drawTypePieChart();
    },
    async selectedMonth2() {
      await this.getTypeCityDateStatistics();
      this.drawTypePieChart();
    },
    selectedYear3() {
      this.drawZheChart();
    },
    async selectedCity3() {
      await this.getCityDateStatistics();
      this.drawZheChart();
    },
    async selectedMethod() {
      if (this.selectedMethod === "年份") {
        this.drawZheChart();
      }
      else {
        await this.getCityDateStatistics();
        this.drawZheChart();
      }
    },
    async anv() {
      if (this.anv == "1") {
        await this.getStatusCityDateStatistics();
        this.drawStatusPieChart();
      }
      else if (this.anv == "2") {
        await this.getTypeCityDateStatistics();
        this.drawTypePieChart();
      }
      else if (this.anv == "3") {
        await this.getCityTypeStatistics();
        this.drawTypeZhuChart();
      }
      else if (this.anv == "4") {
        await this.getCityDateStatistics();
        if (this.selectedMethod === "年份") {
          this.drawZheChart();
        }
        else {
          await this.getCityDateStatistics();
          this.drawZheChart();
        }
      }
    },
  },
  async created() {
    await this.getCityStatistics();           // 获取城市及其总的案件数，其内接口返回数据形如{"西安":10,"上海":20,...}
    await this.getStatusCityDateStatistics(); // 获取指定城市指定年月的案件状态，其内接口返回数据形如{"立案":10,"状态":20,...}
    await this.getTypeCityDateStatistics();   // 获取指定城市指定年月的案件类型，其内接口返回数据形如{"抢劫":10,"谋杀":20,...}
    await this.getCityDateStatistics();       // 获取指定城市指定年月的案件数目，其内接口返回数据形如{"2017":{"1":3,"2":5,...},...}
    this.years = Object.keys(this.cityDateStatistics); // 获取年份范围,形如["2017","2018",...]
    this.cityName = Object.keys(this.cityStatistics);  // 获取年份范围,形如["西安","上海",...]

    // 获取案件总数
    this.numCases = this.statusStatistics["立案"] + this.statusStatistics["调查"] + this.statusStatistics["结案"];
    this.drawStatusPieChart(); // 画总案件状态圆饼图
    // this.drawTypePieChart(); // 画总案件类型圆饼图
    // this.drawZheChart(); // 画折线图

    await this.getCityTypeStatistics();
    // this.drawTypeZhuChart();
  },
}
</script>


<style scoped>
.top-content {
  margin-top: 15px;
  display: block;
}

.bottom-content {
  display: block;
}

.cds-select {
  width: 25%;
  height: 35px;
  display: inline-block;
  margin-left: 40px;
  background-color: #f0e6cf;
  border: #c0b7a2;
}

.cdsamount {
  margin-top: 10px;
  margin-left: 40px
}

.content {
  min-width: 1000px;
  min-height: 800px;
  position: relative;
  box-shadow: 0px 0px 10px 2px rgba(123, 103, 75, 0.427);
  background-color: rgba(255, 255, 255, 0.616);
  margin-bottom: 30px;
  display: flex;
  flex-direction: column;
  justify-content: flex-start;
  align-items: center;
  margin: 16px;
  padding: 16px;

  border-radius: 5px;
  box-shadow: #9a9a9a 0px 0px 6px;
  box-shadow: #777777 0px 0px 3px;
  border-top: #0051ff 3px solid;
  border-top: solid 3px transparent;
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

/* 导航条下方边框阴影*/
.main ::v-deep .el-tabs__nav-wrap {
  background-color: #bca77690;
  min-width: 900px;
  box-shadow: 0px 10px 10px 0px #e6dbc190;
}

.main ::v-deep .el-tabs__item.is-active {
  color: #0051ff !important;
  font-size: 20px;
}

.main ::v-deep .el-tabs__item {
  margin-left: 20px;
  color: #ffffff;
  font-size: 20px;
}

/*下方条 */
.main ::v-deep .el-tabs__active-bar {
  background-color: #0051ff !important;
}
</style>