export const navigation = [
  {
    text: 'Home',
    path: '/home',
    icon: 'home',
  },
  {
    text: 'Inventory',
    icon: 'folder',
    items: [
      {
        text: 'Part Master',
        path: '/profile',
        title: 'Part Master',
        component: 'Part Master',
        selector: ''
      },
      {
        text: 'Tasks',
        path: '/tasks',
        title: 'Tasks',
        component: 'Tasks',
        aelector: ''
      }
    ]
  }
];
