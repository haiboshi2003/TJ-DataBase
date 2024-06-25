import { createRouter, createWebHashHistory } from 'vue-router'



const routes = [
    {
        path: '/',
        component: () => import('../WelcomePage.vue')
    },
    {
        path: '/login',
        component: () => import('../components/login/Login.vue')
    },
    {
        path: '/changepassword',
        component: () => import('../components/login/ChangePassword.vue')
    },
    {
        path: '/ForPassword',
        component: () => import('../components/login/ForPassword.vue')
    },
    {
        path: '/ForPassword/success',
        component: () => import('../components/login/CPSuccess.vue')
    },
    {
        path: '/mainMenu',
        component: () => import('../components/Menu/MainMenu.vue'),
        redirect: '/mainMenu/MainPage',
        
        children: [
            {
                path: 'MainPage',
                component: () => import('../components/Menu/MainPage.vue')

            },
            {
                path: 'Register',
                component: () => import('../components/PoliceManagement/MyRegister.vue')

            },
            {
                path: 'ChangePermission',
                component: () => import('../components/PoliceManagement/ChangePermission.vue'),
                props:true,
            },
            {
                path: 'PayrollManagement',
                component: () => import('../components/PoliceManagement/PayrollManagement.vue')

            },
            {
                path: 'PoliceAssessment',
                component: () => import('../components/PoliceManagement/PoliceAssessment.vue')

            },
            {
                path: 'PoliceStationInfoManagement',
                component: () => import('../components/PoliceManagement/PoliceStationInfoManagement.vue')

            },
            {
                path: 'PoliceOfficerInfoManagement',
                component: () => import('../components/PoliceManagement/PoliceOfficerInfoManagement.vue')

            },
            {
                path: 'AttendanceManagement',
                component: () => import('../components/PoliceManagement/AttendanceManagement.vue')

            },
            {
                path: 'VideoManagement',
                component: () => import('../components/PoliceManagement/VideoManagement.vue')

            },
            {
                path: 'AlarmResponseRecordManagement',
                component: () => import('../components/PoliceManagement/AlarmResponseRecordManagement.vue')

            },
            {
                path: 'ApplyForPermission',
                component: () => import('../components/PoliceManagement/ApplyForPermission.vue'),
                props:true,
            },
            {
                path: 'PermissionManage',
                component: () => import('../components/PoliceManagement/PermissionManage.vue'),
                props:true,
            },
            {
                path: 'PositionManage',
                component: () => import('../components/PoliceManagement/PositionManage.vue'),
                props:true,
            },
            {
                path: 'SuspectInfoManagement',
                component: () => import('../components/CaseManagement/SuspectInfoManagement.vue')

            },
            {
                path: 'CriminalRecordManagement',
                component: () => import('../components/CaseManagement/CriminalRecordManagement.vue')

            },
            {
                path: 'VictimInfoManagement',
                component: () => import('../components/CaseManagement/VictimInfoManagement.vue')

            },
            {
                path: 'CaseClassificationManagement',
                component: () => import('../components/CaseManagement/CaseClassificationManagement.vue')

            },
            {
                path: 'CrimeDataStatistics',
                component: () => import('../components/CaseManagement/CrimeDataStatistics.vue')

            },
            {
                path: 'KeyIndividualsManagement',
                component: () => import('../components/CaseManagement/KeyIndividualsManagement.vue')

            },
            {
                path: 'PoliceVehicleManagement',
                component: () => import('../components/EquipmentManagement/PoliceVehicleManagement.vue')

            },
            {
                path: 'PoliceEquipmentManagement',
                component: () => import('../components/EquipmentManagement/PoliceEquipmentManagement.vue')

            },
            {
                path: 'CitizenInfoManagement',
                component: () => import('../components/UrbanResidentmanagement/CitizenInfoManagement.vue')

            },
            {
                path: 'UrbanZoningManagement',
                component: () => import('../components/UrbanResidentmanagement/UrbanZoningManagement.vue')

            },
            {
                path: 'RegionalDispatch',
                component: () => import('../components/UrbanResidentmanagement/RegionalDispatch.vue')

            },            
            {
                path: 'FamilybgCheck',
                component: () => import('../components/UrbanResidentmanagement/FamilybgCheck.vue')

            },
            {
                path: 'CaseInvestigation',
                component: () => import('../components/CaseInvestigation.vue')

            },
        ]
           


    },
    // {
    //     path: '/jwt',
    //     component: () => import('../components/jwt.vue')
    // },
    // {
    //     path: '/404',
    //     component: () => import('../components/My404.vue')
        
    // },

    // {
    //     path: '/menu',
    //     component: () => import('../components/MainMenu.vue'),
    //     redirect: '/menu/ProductionInquiry',  //重定向，访问/a3立即跳转到/a3/student
    //     children: [
    //         {
    //             path: 'ProductionInquiry',
    //             component: () => import('../components/ProductInquiry.vue')

    //         },
    //         {
    //             path: 'liquidation',
    //             component: () => import('../components/liquidation.vue')
    //         },
    //         {
    //             path: 'addproduct',
    //             component: () => import('../components/Add-product.vue')
    //         },
    //         {
    //             path: 'adduser',
    //             component: () => import('../components/AddUser.vue')
    //         },
    //         {
    //             path: 'settlement',
    //             component: () => import('@/components/Business/Settlement/index.vue')
    //         },
    //         {
    //             path: 'transactions',
    //             component: () => import('@/components/Business/Transactions.vue')
    //         },
    //         {
    //             path: 'purchase',
    //             component: () => import('@/components/Business/Purchase.vue')
    //         },
    //         {
    //             path: 'redemption',
    //             component: () => import('@/components/Business/Redemption.vue')
    //         },
    //         {
    //             path: 'viewNav',
    //             component: () => import('../components/ViewNav.vue')
    //         },
    //         {
    //             path: 'searchCustomer',
    //             component: () => import('../components/searchcustomer.vue')
    //         },
    //         {
    //             path: 'bankaccount',
    //             component: () => import('../components/Bank-Account.vue')
    //         }
    //     ]
    // },
    // {
    //     path: '/:pathMatcher(.*)*',
    //     redirect: '/404'
    // }
    
]

const router = createRouter({
    //router格式，hash
    history: createWebHashHistory(),
    routes : routes
    

})

export default router;