<template>
  <el-header class="sub-header" @mousemove="handleMouseMove">
    <div>&nbsp;&nbsp;城区和居民管理&nbsp;>&nbsp;城市地区管理</div>
  </el-header>

  <!--LHM编写用于呈现区域犯罪信息与人口情况-->

  <div class="main" @wheel.passive.stop>
    <div class="content">
      <div class="inputbox">
        <el-text class="noteText" type="primary">城市全称：</el-text>
        <el-input
          class="inputBox"
          v-model="CityName"
          placeholder="请输入城市全称，如：上海市"
          show-word-limit
        />
        <el-button type="primary" @click="fetchDistrictInfo">查询</el-button>
      </div>

      <div class="container">
        <div v-for="result in distictResults" :key="result.districtName">
          <div class="citynametitle">
            <span>{{ result.districtName }}</span>
          </div>
          <div class="charts">
            <div class="top-content">
              <h2 style="text-align: center">案件类型统计饼图</h2>
              <!-- 使用v-for迭代distictResults列表，并为每个result调用drawStatusPieChart方法 -->
              <div
                :id="'typePieChart_' + result.districtName"
                style="width: 400px; height: 400px"
              ></div>
            </div>
            <div class="top-content">
              <h2 style="text-align: center">案件时间统计折线图</h2>
              <!-- 使用v-for迭代distictResults列表，并为每个result调用drawStatusPieChart方法 -->
              <div
                :id="'timeLineChart_' + result.districtName"
                style="width: 400px; height: 400px"
              ></div>
            </div>
          </div>
          <div class="district-box">
            <div>
              <div style="color: #51a1ec; display: inline-block">
                人口:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
              </div>
              {{ result.population }}
            </div>
            <div>
              <div style="color: #51a1ec; display: inline-block">
                犯罪数:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
              </div>
              {{ result.crimeNum }}
            </div>
            <div>
              <div style="color: #51a1ec; display: inline-block">
                犯罪率:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
              </div>
              {{ crimeRate(result) }}
            </div>
          </div>
        </div>
      </div>

      <el-dialog title="提示" v-model="errBox" @close="handleClose"
        >{{ this.errMessage }}
        <el-button
          style="margin-top: 20px; right: 20px; display: block"
          @click="handleClose"
          ><span>返回</span></el-button
        >
      </el-dialog>
    </div>
  </div>
</template>

<script>
import axios from "../../api/request";

import * as echarts from "echarts";

export default {
  data() {
    return {
      CityName: "",
      distictResults: [],
      crimeTypeStatistics: [],
      errBox: false,
      errMessage: "",
    };
  },
  methods: {
    handleMouseMove(event) {
      const x = event.pageX - event.target.offsetLeft;
      const y = event.pageY - event.target.offsetTop;
      event.target.style.setProperty("--x", `${x}px`);
      event.target.style.setProperty("--y", `${y}px`);
    },

    // 计算犯罪率，展示为百分数保留两位小数
    crimeRate(result) {
      return ((result.crimeNum / result.population) * 100).toFixed(2) + "%";
    },
    drawTypePieChart() {
      //对distictResults列表中的每个result进行遍历
      this.distictResults.forEach((result) => {
        // 生成每个饼状图容器的唯一id
        const chartId = "typePieChart_" + result.districtName;
        // 初始化echarts实例，并传入对应的id
        const pieChart = echarts.init(document.getElementById(chartId));

        const option = {
          tooltip: {
            trigger: "item",
          },
          legend: {
            top: "5%",
            left: "center",
          },
          series: [
            {
              name: "Access From",
              type: "pie",
              radius: ["40%", "70%"],
              avoidLabelOverlap: false,
              itemStyle: {
                borderRadius: 10,
                borderColor: "#fff",
                borderWidth: 2,
              },
              label: {
                show: false,
                position: "center",
              },
              emphasis: {
                label: {
                  show: true,
                  fontSize: 20,
                  fontWeight: "bold",
                },
              },
              labelLine: {
                show: false,
              },
              data: [
                { value: result.crimeTypeStatistic[0], name: "强奸" },
                { value: result.crimeTypeStatistic[1], name: "抢劫" },
                { value: result.crimeTypeStatistic[2], name: "故意伤害" },
                { value: result.crimeTypeStatistic[3], name: "盗窃" },
                { value: result.crimeTypeStatistic[4], name: "诈骗" },
                { value: result.crimeTypeStatistic[5], name: "谋杀" },
              ],
            },
          ],
        };
        option && pieChart.setOption(option);
      });
    },

    drawTimeLineChart() {
      this.distictResults.forEach((result) => {
        console.log(result.districtName);
        const chartId = "timeLineChart_" + result.districtName;

        console.log(chartId);
        const zheChart = echarts.init(document.getElementById(chartId));
        const years = Object.keys(result.districtCrimeTimeStatistic);
        const data = Object.values(result.districtCrimeTimeStatistic);

        const option = {
          xAxis: {
            type: "category",
            data: years,
          },
          yAxis: {
            type: "value",
          },
          series: [
            {
              data: data,
              type: "line",
            },
          ],
        };
        option && zheChart.setOption(option);
      });
    },

    async fetchDistrictInfo() {
      try {
        if (this.CityName === "") {
          this.errMessage = "请输入城市名称！";
          this.errBox = true;
          return;
        }
        const response = await axios.get(
          `/api/CityCrimeInfo?CityName=${encodeURIComponent(this.CityName)}`
        );
        console.log(response.data);
        this.distictResults = response.data;
        await new Promise((resolve) => setTimeout(resolve, 300));
        this.drawTypePieChart();
        this.drawTimeLineChart();
        if (response.data.length == 0) {
          this.errMessage = "暂无该城市城区记录！";
          this.errBox = true;
        }
        console.log("调用结束");
      } catch (error) {
        this.errMessage = "未搜索到相关城市信息，请检查输入！";
        this.errBox = true;
        console.log("fetchDistrictInfo出错！" + error);
      }
    },
    handleClose() {
      this.errBox = false;
    },
  },
};
</script>

<style scoped>
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
  margin-left: 40px;
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
.noteText {
  font-size: 18px;
  text-align: center;
  margin-left: -40px;
  min-width: 100px;
  width: 7vw;
  display: inline-block;
}
/*输入框可显示长度为250px*/
.inputBox {
  width: 250px;
  margin-left: 10px;
  margin-right: 10px;
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
/*inputbox中元素横向并列，居中，元素间横向间隔10px */
.inputbox {
  display: flex;
  justify-content: center;
  align-items: center;
  margin-top: 20px;
}
.main {
  margin-top: 10vh;
  max-height: 300vh;
  overflow: auto;
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
.district-box {
  margin-top: -20px;
  width: 280px;
  margin-left: 250px;
  font-size: 22px;
  line-height: 35px;
  box-shadow: 10px 10px 8px 0px rgba(209, 209, 209, 0.756);
}
.charts {
  display: inline-block;
  margin-top: 20px;
  width: 100%;
}
.top-content {
  margin-top: 15px;
  width: 50%;
  display: inline-block;
}
.citynametitle {
  margin-top: 50px;

  width: 100%;
  height: 40px;
  padding: 0 30px;
  line-height: 60px;
  text-align: center;
  position: relative;
  appearance: none;
  background: #51a1ec;
  box-shadow: 0px 10px 8px 0px rgba(209, 209, 209, 0.756);
  border: none;
  color: white;
  font-size: 25px;
  outline: none;
  overflow: hidden;
  border-radius: 0px;
}
.citynametitle span {
  position: relative;
  top: -20%;
}
</style>
