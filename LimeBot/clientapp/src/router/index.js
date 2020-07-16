import Vue from 'vue';
import VueRouter from 'vue-router';
import DefaultLayout from '../layouts/DefaultLayout.vue';
import DashboardLayout from '../layouts/DashboardLayout.vue';
import Home from '../views/Home.vue';
import Commands from '../views/Commands.vue';
import Dashboard from '../views/dashboard/Home.vue';
import GeneralSettings from '../views/dashboard/GeneralSettings.vue';
import JoinLeave from '../views/dashboard/JoinLeave.vue';
import Servers from '../views/dashboard/Servers.vue';
import Logs from '../views/dashboard/Logs.vue';
import ReactionRoles from '../views/dashboard/ReactionRoles.vue';
import Ranking from '../views/dashboard/Ranking.vue';
import Leaderboard from '../views/Leaderboard';


import NotFound from '../views/NotFound.vue';

Vue.use(VueRouter);

const routes = [
  {
    path: '/manage',
    component: DashboardLayout,
    children: [
      {
        path: '',
        component: Servers,
        meta: {nav: false, title: 'Manage servers'}
      },
      {
        path: ':id',
        component: Dashboard,
        meta: {title: 'Dashboard'}
      },
      {
        path: ':id/general',
        component: GeneralSettings,
        meta: {title: 'General settings'}
      },
      {
        path: ':id/join-leave',
        component: JoinLeave,
        meta: {title: 'Join / leave actions'}
      },
      {
        path: ':id/logs',
        component: Logs,
        meta: {title: 'Logs'}
      },
      {
        path: ':id/reaction-roles',
        component: ReactionRoles,
        meta: {title: 'Reaction role settings'}
      },
      {
        path: ':id/ranking',
        component: Ranking,
        meta: {title: 'XP / Leveling settings'}
      }
    ]
  },
  {
    path: '/logout',
    beforeEnter: (to, from, next)=>{
      Vue.$cookies.remove('token');
      next('/');
    }
  },
  {
    path: '/invite',
    beforeEnter: (to)=>{
      window.location.href = to.query.id ? Vue.$api.path(`redirect/invite/?id=${to.query.id}`) : Vue.$api.path('redirect/invite');
    }
  },
  {
    path: '/callback',
    beforeEnter: (to, from, next) => {
      if(to.query.token) {
        Vue.$cookies.set('token', to.query.token);
        next(to.query.redirect || '/manage');
      } else {
        next('/');
      }
    }
  },
  {
    path: '/',
    component: DefaultLayout,
    children: [
      {
        path: '/',
        name: 'Home',
        component: Home
      },
      {
        path: '/commands',
        name: 'Commands',
        component: Commands,
        meta: {title: 'Commands'}
      },
      {
        path: '/leaderboard/:id',
        component: Leaderboard,
        meta: {title: 'Leaderboard'}
      },
      {
        path: '*',
        component: NotFound,
        meta: {title: 'Not found'}
      }
    ]
  }
];

const router = new VueRouter({
  mode: 'history',
  base: process.env.BASE_URL,
  routes
});

const siteName = 'Lime Bot';
router.afterEach((to) => {
    Vue.nextTick(() => {
        document.title = to.meta.title ? to.meta.title + ' - ' + siteName : siteName;
    });
});

export default router;
