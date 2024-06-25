<template>
  <div>
    <transition name="all">
      <div class="container">
        <div
          class="textCtrlL"
          
        >
          <div :key="content.id" v-show="toShow">
            身份证号:<br />
            {{ content.id }}<br /><br />
            犯罪类型:<br />
            <li
              v-for="item in content.crimeType"
              :key="item.id"
              v-show="toShow"
            >{{ item }}</li>
          </div>
        </div>

        <div class="circleBack">
          <img
            :src="imgUrl"
            class="circleCtrl"
            @mouseenter="changeTextStatus"
            @mouseleave="changeTextStatus"
          />
        </div>

        <div
          class="textCtrlR"
        >
          <div :key="content.name">
            {{ this.name }}
          </div>
          <div v-show="toShow">
            <br/>{{ content.gender == "M" ? "男" : "女" }}
          </div>
        </div>
      </div>
    </transition>
  </div>
</template>

<script>
export default {
  data() {
    return {
      name: "",
      toShow: true,
    };
  },
  props: ["imgUrl", "content"],
  methods: {
    changeTextStatus() {
      this.isShow = !this.isShow;
    },
  },
  watch: {
    content: {
      handler() {
        if (this.content.name == "") {
          this.name = "未登记";
          this.toShow = false;
        }
        else{
          this.name=this.content.name;
        }
      },
      immediate: true,
    },
  },
};
</script>

<style scoped>
.container {
  width: 400px;
  height: 160px;
  position: relative;
  overflow: hidden;
  background-color: #d6f7feba;

  border-radius: 5%;
  display: flex;
  justify-content: flex-start;
  align-items: center;
}
.circleBack {
  width: 150px;
  height: 150px;
  position: relative;
  border-radius: 50%;
  overflow: hidden;
  background: linear-gradient(315deg, #42bdd3 25%, #64aaff);

  display: flex;
  justify-content: center;
  align-items: center;
}
.circleCtrl {
  width: 90%;
  height: 90%;
  position: relative;
  border-radius: 50%;
  overflow: hidden;
  background: #eceff1;

  display: flex;
  justify-content: center;
  align-items: center;
}
.textCtrlL {
  width: 170px;
  height: 100%;
  font-size: 16px;

  display: flex;
  justify-content: center;
  align-items: center;
}
.textCtrlR {
  width: 70px;
  height: 100%;
  font-size: 16px;

  display: flex;
  flex-direction: column; 
  justify-content: center;
  align-items: center;
}
</style>