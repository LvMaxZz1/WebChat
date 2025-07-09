// 聊天分组类型
export interface ChatGroup {
  id: string; // 分组唯一标识
  name: string; // 分组名称
}

// 聊天项类型
export interface ChatItem {
  id: string; // 聊天唯一标识
  groupId: string; // 所属分组id
  title: string; // 聊天标题
  lastMessage: string; // 最后一条消息内容
  unreadCount: number; // 未读消息数
}

// 用户类型
export interface UserInfo {
  id: string; // 用户唯一标识
  name: string; // 用户名
  avatar: string; // 头像url
}
