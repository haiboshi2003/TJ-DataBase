<template>
  <el-container
    class="home-container"
    :style="{
      height: containerHeight + 'px',
      backgroundSize: '100% ' + containerHeight + 'px',
    }"
    @wheel="handleScroll"
  >
    <!-- 头部 -->
    <el-header
      style="height: 70px; padding: 0; text-align: justify; hyphens: auto"
    >
      <div class="headpic">
        <div class="header1">
          <img class="logo" src="../../assets/image-2.png" />
          <div class="title">&nbsp;&nbsp;警务处理系统</div>
          <div class="button-container">
            <el-button type="primary" @click="handleSelfInfoClick">
              个人信息
            </el-button>
            <el-button type="primary" @click="Exit"> 退出账号 </el-button>
          </div>
        </div>
      </div>
    </el-header>

    <!-- 页面主体 -->
    <el-container>
      <!-- 导航栏 -->
      <el-aside width="200px">
        <el-menu
          router
          class="el-menu-vertical-demo"
          default-active="/mainMenu/MainPage"
          background-color="transparent"
          text-color="#ffffff"
          active-text-color="#ffd04b"
        >
          <el-menu-item
            v-if="myAuthority >= 1"
            index="/mainMenu/MainPage"
            @click="scrollToTop"
          >
            <img
              class="nav-icon-for-menu-item"
              src="../../assets/logos/MainPage.png"
              alt="Icon"
            />
            &nbsp;主&nbsp;页
          </el-menu-item>

          <el-sub-menu index="1-1">
            <!-- 警员管理 -->
            <template #title>
              <span>
                <img
                  class="nav-icon-for-sub-menu"
                  src="../../assets/logos/PoliceManagement.png"
                  alt="Icon"
                />
                警员管理</span
              >
            </template>
            <el-menu-item
              v-if="myAuthority >= 1"
              index="/mainMenu/Register"
              @click="scrollToTop"
            >
              <img
                class="nav-icon-for-menu-item"
                src="../../assets/logos/Register.png"
                alt="Icon"
              />
              注&nbsp;册
            </el-menu-item>
            <el-menu-item
              v-if="myAuthority >= 2"
              index="/mainMenu/ChangePermission"
              @click="scrollToTop"
            >
              <img
                class="nav-icon-for-menu-item"
                src="../../assets/logos/ChangePermission.png"
                alt="Icon"
              />
              警员权限修改
            </el-menu-item>
            <el-menu-item
              v-if="myAuthority >= 5"
              index="/mainMenu/PermissionManage"
              @click="scrollToTop"
            >
              <img
                class="nav-icon-for-menu-item"
                src="../../assets/logos/PermissionManage.png"
                alt="Icon"
              />
              修改申请管理
            </el-menu-item>
            <el-menu-item
              v-if="myAuthority >= 1"
              index="/mainMenu/PoliceAssessment"
              @click="scrollToTop"
            >
              <img
                class="nav-icon-for-menu-item"
                src="../../assets/logos/PoliceAssessment.png"
                alt="Icon"
              />
              警员考核管理
            </el-menu-item>
            <el-menu-item
              v-if="myAuthority >= 4"
              index="/mainMenu/PoliceStationInfoManagement"
              @click="scrollToTop"
            >
              <img
                class="nav-icon-for-menu-item"
                src="../../assets/logos/PoliceStationInfoManagement.png"
                alt="Icon"
              />
              警局信息管理
            </el-menu-item>
            <el-menu-item
              v-if="myAuthority >= 0"
              index="/mainMenu/PoliceOfficerInfoManagement"
              @click="scrollToTop"
            >
              <img
                class="nav-icon-for-menu-item"
                src="../../assets/logos/PoliceOfficerInfoManagement.png"
                alt="Icon"
              />
              警员信息管理
            </el-menu-item>
            <el-menu-item
              v-if="myAuthority >= 1"
              index="/mainMenu/AttendanceManagement"
              @click="scrollToTop"
            >
              <img
                class="nav-icon-for-menu-item"
                src="../../assets/logos/AttendanceManagement.png"
                alt="Icon"
              />
              警员出勤管理
            </el-menu-item>
            <el-menu-item
              v-if="myAuthority >= 1"
              index="/mainMenu/VideoManagement"
              @click="scrollToTop"
            >
              <img
                class="nav-icon-for-menu-item"
                src="../../assets/logos/VideoManagement.png"
                alt="Icon"
              />
              执法录像管理
            </el-menu-item>
            <el-menu-item
              v-if="myAuthority >= 1"
              index="/mainMenu/AlarmResponseRecordManagement"
              @click="scrollToTop"
            >
              <img
                class="nav-icon-for-menu-item"
                src="../../assets/logos/AlarmResponseRecordManagement.png"
                alt="Icon"
              />
              接警记录管理
            </el-menu-item>

            <el-menu-item
              index="/mainMenu/PayrollManagement"
              v-if="myAuthority >= 4"
              @click="scrollToTop"
            >
              <img
                class="nav-icon-for-menu-item"
                src="../../assets/logos/PayrollManagement.png"
                alt="Icon"
              />
              薪水管理
            </el-menu-item>
          </el-sub-menu>

          <!-- 案件管理 -->
          <el-sub-menu v-if="myAuthority >= 1" index="1-2">
            <template #title>
              <span>
                <img
                  class="nav-icon-for-sub-menu"
                  src="../../assets/logos/CaseManagement.png"
                  alt="Icon"
                />
                案件管理</span
              >
            </template>

            <el-menu-item
              index="/mainMenu/SuspectInfoManagement"
              @click="scrollToTop"
            >
              <img
                class="nav-icon-for-menu-item"
                src="../../assets/logos/SuspectInfoManagement.png"
                alt="Icon"
              />
              嫌疑人信息管理
            </el-menu-item>
            <el-menu-item
              index="/mainMenu/CriminalRecordManagement"
              @click="scrollToTop"
            >
              <img
                class="nav-icon-for-menu-item"
                src="../../assets/logos/CriminalRecordManagement.png"
                alt="Icon"
              />
              犯罪记录管理
            </el-menu-item>
            <el-menu-item
              index="/mainMenu/VictimInfoManagement"
              @click="scrollToTop"
            >
              <img
                class="nav-icon-for-menu-item"
                src="../../assets/logos/VictimInfoManagement.png"
                alt="Icon"
              />
              受害人信息管理
            </el-menu-item>
            <el-menu-item
              index="/mainMenu/CaseClassificationManagement"
              @click="scrollToTop"
            >
              <img
                class="nav-icon-for-menu-item"
                src="../../assets/logos/CaseClassificationManagement.png"
                alt="Icon"
              />
              案件分类与管理
            </el-menu-item>
            <el-menu-item
              index="/mainMenu/CrimeDataStatistics"
              @click="scrollToTop"
            >
              <img
                class="nav-icon-for-menu-item"
                src="../../assets/logos/CrimeDataStatistics.png"
                alt="Icon"
              />
              犯罪数据统计
            </el-menu-item>
            <el-menu-item
              index="/mainMenu/KeyIndividualsManagement"
              @click="scrollToTop"
            >
              <img
                class="nav-icon-for-menu-item"
                src="../../assets/logos/KeyIndividualsManagement.png"
                alt="Icon"
              />
              重点人员统计
            </el-menu-item>
          </el-sub-menu>

          <!-- 装备管理 -->
          <el-sub-menu v-if="myAuthority >= 1" index="1-3">
            <template #title>
              <img
                class="nav-icon-for-menu-item"
                src="../../assets/logos/EquipmentManagement.png"
                alt="Icon"
              />
              <span>装备管理</span>
            </template>
            <el-menu-item
              index="/mainMenu/PoliceVehicleManagement"
              @click="scrollToTop"
            >
              <img
                class="nav-icon-for-menu-item"
                src="../../assets/logos/PoliceVehicleManagement.png"
                alt="Icon"
              />
              警车管理
            </el-menu-item>
            <el-menu-item
              index="/mainMenu/PoliceEquipmentManagement"
              @click="scrollToTop"
            >
              <img
                class="nav-icon-for-menu-item"
                src="../../assets/logos/PoliceEquipmentManagement.png"
                alt="Icon"
              />
              警械管理
            </el-menu-item>
          </el-sub-menu>

          <!-- 城区和居民管理 -->

          <el-sub-menu v-if="myAuthority >= 1" index="1-4">
            <template #title>
              <img
                class="nav-icon-for-menu-item"
                src="../../assets/logos/UrbanResidentmanagement.png"
                alt="Icon"
              />
              <span>城区和居民管理</span>
            </template>

            <el-menu-item
              index="/mainMenu/CitizenInfoManagement"
              @click="scrollToTop"
            >
              <img
                class="nav-icon-for-menu-item"
                src="../../assets/logos/CitizenInfoManagement.png"
                alt="Icon"
              />
              公民户籍管理
            </el-menu-item>
            <el-menu-item
              index="/mainMenu/UrbanZoningManagement"
              @click="scrollToTop"
            >
              <img
                class="nav-icon-for-menu-item"
                src="../../assets/logos/UrbanZoningManagement.png"
                alt="Icon"
              />
              城市地区管理
            </el-menu-item>
            <el-menu-item
              index="/mainMenu/RegionalDispatch"
              @click="scrollToTop"
            >
              <img
                class="nav-icon-for-menu-item"
                src="../../assets/logos/RegionalDispatch.png"
                alt="Icon"
              />
              城市调度管理
            </el-menu-item>
            <el-menu-item index="/mainMenu/FamilybgCheck" @click="scrollToTop">
              <img
                class="nav-icon-for-menu-item"
                src="../../assets/logos/FamilybgCheck.png"
                alt="Icon"
              />
              家族背景调查
            </el-menu-item>
          </el-sub-menu>

          <el-menu-item
            v-if="myAuthority >= 1"
            index="/mainMenu/CaseInvestigation"
            @click="scrollToTop"
          >
            <img
              class="nav-icon-for-menu-item"
              src="../../assets/logos/CaseInvestigation.png"
              alt="Icon"
            />
            案件办理
          </el-menu-item>
        </el-menu>
      </el-aside>

      <!-- 页面主体 -->
      <el-main class="background">
        <!-- 无登录权限提示框 -->
        <el-dialog v-model="loginErr" @close="handleClose" title="提示"
          >没有登录权限，请重新登录！
          <el-button
            style="display: block; margin-top: 20px"
            @click="handleClose"
            >确认</el-button
          >
        </el-dialog>

        <router-view></router-view>
      </el-main>
    </el-container>
  </el-container>

  <!-- 账户信息展示 -->
  <el-drawer v-model="drawer2" :direction="direction">
    <template #header> </template>
    <template #default>
      <div class="myInfo">
        <span>警号：{{ myPoliceNumber }}</span>
      </div>

      <div class="myInfo">
        <span>姓名：{{ myName }}</span>
      </div>

      <div class="myInfo">
        <span>性别：{{ mySex }}</span>
      </div>

      <div class="myInfo">
        <span>出生日期：{{ myBirthday }}</span>
      </div>

      <div class="myInfo">
        <span>民族：{{ myPeoples }}</span>
      </div>

      <div class="myInfo">
        <span>联系电话：{{ myPhone }}</span>
      </div>

      <div class="myInfo">
        <span>状态：{{ myStatus }}</span>
      </div>

      <div class="myInfo">
        <span>职务：{{ myPosition }}</span>
      </div>

      <div class="myInfo">
        <span>权限等级：{{ myAuthority }}</span>
      </div>
    </template>
    <template #footer> </template>
  </el-drawer>
</template>

<script lang="js" setup>
import { ref } from "vue";
import axios from '../../api/request';


const containerHeight= ref(2000) // 初始化容器高度
const minHeight= ref(2000) // 最小高度
const maxScrollHeight= ref(3500) // 最大滚动高度，控制界面的延伸
// const mytitle= "注册"

const drawer2 = ref(false);
const direction = ref("rtl");
const username = ref("1234567");
const password = ref("44554");

const myPoliceNumber = localStorage.getItem('policeNumber');
const myAuthority = localStorage.getItem('authority');
var loginErr = localStorage.getItem('loginErr') === 'true';
const err= ref("警员不存在！");
const policemenInfo=ref([]);

const myName=ref("");
const mypolicemenIDNum=ref("");
const mypolicemenYear=ref("");
const mySex=ref("");
const myBirthday=ref("");
const myPeoples =ref("");
const myPhone=ref("");
const myStatus=ref("");
const myPosition=ref("");
function handleSelfInfoClick(event)
{
      drawer2.value = true;
}
function handleScroll(event) {
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
}
axios.get(
          `/api/policemenInfo?policemenID=${encodeURIComponent(
            myPoliceNumber
          )}&policemenName=${encodeURIComponent(
            myName.value
          )}&policemenIDNum=${encodeURIComponent(
            mypolicemenIDNum.value
          )}&policemenYear=${encodeURIComponent(
            mypolicemenYear.value
          )}&policemenSex=${encodeURIComponent(
            mySex.value
          )}&policemenNation=${encodeURIComponent(
            myPeoples.value
          )}&policemenStatus=${encodeURIComponent(
            myStatus.value
          )}&policemenPosition=${encodeURIComponent(myPosition.value)}`
        )
        .then((res) => {
          policemenInfo.value = res.data;
          //console.log(policemenInfo.value);
            if (policemenInfo.value[0].gender === "F") {
              policemenInfo.value[0].gender = "女";
            } else if (policemenInfo.value[0].gender === "M") {
              policemenInfo.value[0].gender = "男";
            }
            if (Array.isArray(policemenInfo.value) && policemenInfo.value.length > 0) {
        myName.value=policemenInfo.value[0].name;
        mySex.value=policemenInfo.value[0].gender;
        myBirthday.value=policemenInfo.value[0].birthday;
        myPeoples.value =policemenInfo.value[0].nation;
        myPhone.value=policemenInfo.value[0].phone;
        myStatus.value=policemenInfo.value[0].status;
        myPosition.value=policemenInfo.value[0].position;
        console.log(policemenInfo)

        localStorage.setItem("name",  myName.value);
        localStorage.setItem("policeNumber", myPoliceNumber);

        }
        })
        .catch((err) => {
          console.log(err);
        });

function scrollToTop() {
  containerHeight.value = 1500;
  window.scrollTo({
  top: 0,
  behavior: 'smooth'
  })
}
function Exit() {
  localStorage.setItem('loginErr','true');
  localStorage.setItem('policeNumber', '');
  localStorage.setItem('authority', 0);
  window.location.href='http://118.25.23.3';
}
function handleClose() {
  window.location.href='http://118.25.23.3';
  loginErr = false;
}
</script>

<style scoped>
.myInfo {
  margin-left: 15%;
  margin-top: 10%;
}
.nav-icon-for-sub-menu {
  max-width: 30%;
  max-height: 30%;
  vertical-align: middle;
  margin-right: 5px; /* Optional: Add some spacing between icon and text */
}
.nav-icon-for-menu-item {
  max-width: 60%;
  max-height: 60%;
  vertical-align: middle;
  margin-right: 5px; /* Optional: Add some spacing between icon and text */
}

.home-container {
  width: 100%;
  min-width: 100%;
  overflow: hidden;
  background-color: #f0f0f0;
  transition: height 0.3s ease; /* 添加过渡效果 */
}

.el-menu-item.is-active {
  background-color: #1c80de !important;
  /* color: red; 设置选中时的字体颜色 */
}

.menu-link {
  text-decoration: none;
  color: #ffffff;
}

.background {
  background-image: url("../../assets/bg.png"); /* 替换为你的背景图路径 */
  background-size: cover; /* 根据需要调整背景图的尺寸适应方式 */
  background-attachment: fixed;
  background-position: center;
  background-repeat: no-repeat;
  /* 其他背景相关样式，例如背景颜色、背景重复等，可根据需要添加 */
}

.el-aside {
  background-color: #1620a9;
  background-image: url("../../assets/sdtest.jpg"); /* 替换为你的背景图路径 */

  background-attachment: fixed;
  background-repeat: no-repeat;
}
.headpic {
  background: #ffffff;
  width: 100%;
  position: relative;
  overflow: hidden;
}

.header1 {
  background: #1f2cdf;
  background-image: url("../../assets/hdtest.jpg"); /* 替换为你的背景图路径 */
  background-size: contain;
  background-position: right top; /* 背景图靠左上角 */
  background-repeat: no-repeat;
  width: 100%;
  height: 70px;
  position: relative;

  left: 0;
  display: flex;
  align-items: center;
  padding-left: 20px;
}

.button-container {
  margin-left: auto;
  margin-right: 20px;
}

.title {
  color: #ffffff;
  text-align: left;
  font: 400 36px "Inter", sans-serif;
  display: inline-block;
}
</style>
