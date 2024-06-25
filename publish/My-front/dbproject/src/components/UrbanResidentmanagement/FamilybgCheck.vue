<template>
  <div class="container">
    <!-- main window -->
    <!-- element:query inputbox and confirm button-->
    <div class="queryBox" :class="{ upBox: isGraphContainerVisible }">
      <input
        type="text"
        v-model="inputID"
        :placeholder="placeholder"
        @focus="changeInputStatus"
        @blur="changeInputStatus"
        @keydown.enter="query"
        class="inputBox"
        :class="{ errorAnime: isError }"
      />
      <button @click="query" class="button">查询</button>
    </div>
    <!-- element:family graph container -->
    <!-- graph is where criminal info is shown -->
    <div v-if="readyToRenderGraph" class="graphContainer">
      <!-- this part is for info render,
     step1: calc position and sequential for each info
     step2: render by sequential -->
      <transitionGroup name="fade" tag="ul">
        <div
          class="imgCtrl"
          v-for="(item, index) in this.infoList"
          :style="getPos(index)"
          :key="index"
          :id="index"
          @mouseenter="setLineObvious(index)"
          @mouseleave="setLineDefault(index)"
          @click="restart(index)"
        >
          <CriminalInfo
            :imgUrl="item.gender === 'F' ? femaleUrl : maleUrl"
            :content="item"
          />
        </div>
      </transitionGroup>
    </div>
  </div>
</template>

<script>
import CriminalInfo from "./CriminalInfo.vue";
import axios from "../../api/request";

import LeaderLine from "leader-line";
export default {
  components: {
    CriminalInfo,
  },
  data() {
    return {
      inputID: "",
      content: [],
      maleUrl: require("@/assets/male.png"),
      femaleUrl: require("@/assets/female.png"),
      placeholder: "请输入居民身份证号以进行家族背景调查",
      isGraphContainerVisible: false,
      readyToRenderGraph: false,
      isError: false, // 控制错误动画
      // 以下为渲染信息相关
      curInfo: 0, // 当前加载位置
      infoList: [], // 用于存储渲染的信息
      lineList: [], // 用于渲染线条
      hasFlushed: false,
    };
  },
  watch: {
    content: {
      handler() {
        this.infoList = [];
        this.lineList = [];
        this.curInfo = 0;
        setTimeout(() => {
          this.readyToRenderGraph = true;
        }, 500);
        // 设置infoList
        setInterval(() => {
          if (this.curInfo < this.content.people.length) {
            this.infoList.push(this.content.people[this.curInfo]);
            this.curInfo++;
          } else if (this.lineList.length === 0 && !this.hasFlushed) {
            // 此时人物已全部添加
            for (
              var index = 1;
              index < this.infoList.length && index <= 2;
              index++
            ) {
              const myline = new LeaderLine(
                document.getElementById("" + 0),
                document.getElementById("" + index),
                {
                  startSocket: "top",
                  endSocket: "bottom",
                  path: "magnet",
                  size: 3,
                  dash: { animation: false },
                  gradient: {
                    startColor: "#2e17c3",
                    endColor: "#1df3f9",
                    animation: "true",
                  },
                  endPlugColor: "#1df3f9",
                }
              );
              this.lineList.push(myline);
            }
            for (
              var childIndex = 3;
              childIndex < this.infoList.length;
              childIndex++
            ) {
              const myline = new LeaderLine(
                document.getElementById("" + 0),
                document.getElementById("" + childIndex),
                {
                  startSocket: "bottom",
                  endSocket: "top",
                  path: "magnet",
                  size: 3,
                  dash: { animation: false },
                  gradient: {
                    startColor: "#2e17c3",
                    endColor: "#1df3f9",
                  },
                  endPlugColor: "#1df3f9",
                }
              );
              this.lineList.push(myline);
            }
          }
        }, 400);
      },
    },
  },
  methods: {
    restart(index) {
      if (index == 0 || this.infoList[index].name == "") return;
      // 换中心人物的处理函数
      /* 准备工作区 */
      this.flushLine(); // 清空指示线
      this.inputID = this.infoList[index].id;

      /*************/
      /* 重置工作区 */
      this.query();
      /*************/
    },
    getPos(index) {
      let width = this.$el.offsetWidth;
      let height = this.$el.offsetHeight;
      let top = "0%";
      let left = "0%";
      if (index == 0) {
        // 中心人物
        top = `${(height / 4) * 2 - 100}px`;
        left = `${width / 2 - 250}px`;
      } else if (index == 1 || index == 2) {
        // 父母
        top = `${height / 4 - 100 - 50}px`;
        left = `${(width / 100) * index * 30 - 200}px`;
      } else {
        // 孩子关系
        let leftChild = this.content.people.length - 3;
        top = `${(height / 4) * 3 - 80}px`;
        left = `${(width / (leftChild + 1)) * (index - 2) - 200}px`;
      }
      return {
        top,
        left,
      };
    },
    startErrorAnime() {
      this.isError = true;
      // 重置显示已有的输入内容为空
      this.inputID = "";
      this.placeholder = "不存在此犯人，请重新输入";
      setTimeout(() => {
        this.isError = false;
        this.inputID = "";
        this.placeholder = "";
      }, 500);
    },
    query() {
      axios
        .post(`/api/FamilybgCheck`, {
          InputText: this.inputID,
        })
        .then((response) => {
          if (response.data.people.length !== 0) {
            // 有数据
            this.flushLine(); //2023.9.3 测试一下效果
            this.hasFlushed = false;
            this.content = response.data;
            this.isGraphContainerVisible = true;
            this.isError = false;
          } else {
            this.startErrorAnime();
            this.flushLine();
            location.reload(); //2023.9.3 刷新界面
            this.readyToRenderGraph = false;
          }
        })
        .catch((error) => {
          console.error(error);
          alert("网络错误，请重试");
        });
    },
    changeInputStatus() {
      if (this.placeholder === "") {
        this.placeholder = "请输入居民身份证号以进行家族背景调查";
      } else {
        this.placeholder = "";
      }
    },
    flushLine() {
      this.lineList.forEach((line) => line.remove());
      this.lineList = [];
      this.hasFlushed = true;
    },
    setLineObvious(id) {
      if (id > this.lineList.length || id == 0) return;
      this.lineList[id - 1].setOptions({ size: 6, dash: { animation: true } });
    },
    setLineDefault(id) {
      if (id > this.lineList.length || id == 0) return;
      this.lineList[id - 1].setOptions({ size: 3, dash: { animation: false } });
    },
  },
  beforeRouteLeave() {
    console.log("end");
    this.flushLine();
  },
};
</script>

<style scoped>
.queryBox {
  width: 100%;
  height: 100px;
  position: relative;
  top: 40vh;
  overflow: hidden;
  display: flex;
  justify-content: center;
  align-items: center;
  transition: transform 0.5s ease;
}

.upBox {
  transform: translateY(-40vh);
}
.container {
  background: #ffffff;
  width: 100%;
  height: 100vh;
  position: relative;
  overflow: hidden;

  display: flex;
  flex-direction: column;
  justify-content: flex-start;
  align-items: center;
}
.inputBox {
  width: 350px;
  height: 50px;
  text-align: center;
  border-radius: 16px;
  font-size: 16px;
  background: #ffffff;
  padding: 3px;
}
.inputBox:focus {
  outline: none;
  border: 3px rgba(25, 167, 255, 0.644) solid;
  box-shadow: 0 0 6px rgba(25, 167, 255, 0.644);
}
.inputBox:not(:focus) {
  outline: none;
  border: 1px rgba(158, 158, 158, 0.943) solid;
  box-shadow: 0 0 0px rgba(158, 158, 158, 0.943);
}
.inputBox:hover:not(:focus) {
  outline: none;
  border: 1px rgba(158, 158, 158, 0.943) solid;
  box-shadow: 0 0 5px rgba(214, 214, 214, 0.943);
}
.button {
  width: 70px;
  height: 55px;
  text-align: center;
  margin-left: 12px;
  border-radius: 16px;
  font-size: 16px;
  color: #ffffff;
  background: #00aeff;
}
.imgCtrl {
  width: 400px;
  height: 160px;
  position: absolute;
}
.graphContainer {
  width: 100%;
  /* 高度：填满剩余屏幕 */
  height: calc(100% - 100px);
  position: relative; /* 子元素相对于父元素绝对位置 */
  top: 9px;
  overflow: hidden;
}

.imgAnime-enter-active {
  transition: opacity 0.5s ease, transform 0.5s ease, width 0.5s ease,
    height 0.5s ease; /* 添加宽度和高度的过渡效果 */
}
.imgAnime-leave-active {
  transition: opacity 0.5s ease, transform 0.5s ease, width 0.5s ease,
    height 0.5s ease; /* 添加宽度和高度的过渡效果 */
}
.imgAnime-enter {
  opacity: 0;
  transform: translateY(100%) scale(0); /* 添加初始的缩放效果 */
}
.imgAnime-leave-to {
  opacity: 0;
  transform: translateY(100%) scale(0); /* 添加最终的缩放效果 */
}

/* animation for shaking */
.errorAnime {
  outline: none;
  border: 3px rgba(255, 56, 56, 0.943) solid !important;
  animation: shake 0.2s infinite;
}
@keyframes shake {
  0% {
    transform: translateX(0);
  }
  25% {
    transform: translateX(-5px);
  }
  50% {
    transform: translateX(0);
  }
  75% {
    transform: translateX(5px);
  }
  100% {
    transform: translateX(0);
  }
}

/* animation for info show */
.fade-enter-active,
.fade-leave-active {
  transition: all 0.4s ease;
}

/* 2. 声明进入和离开的状态 */
.fade-enter-from,
.fade-leave-to {
  opacity: 0;
  transform: scaleY(0.01) translate(30px, 0);
}

.slash {
  width: 0;
  height: 0;
  border-top: 50px solid transparent;
  border-bottom: 50px solid transparent;
  border-left: 100px solid black;
}
</style>
