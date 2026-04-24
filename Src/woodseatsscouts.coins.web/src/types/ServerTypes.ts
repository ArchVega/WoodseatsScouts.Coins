export type MemberBase = {
  id: number;
  code: string
  number: number
  firstName: string
  lastName: string
  fullName: string
  scoutGroupId: number
  sectionId: string
  computedImagePath: string
  clientComputedImageUri: string
  memberCode: string
  hasImage: boolean
  memberNumber: number
}

export type MemberDto = MemberBase & {
  scoutGroupName: string
  sectionName: string
  clue1State: string
  clue2State: string
  clue3State: string
  isDayVisitor: boolean
  hasImage: boolean
}

export type MemberPointsSummaryDto = MemberBase & {
  scoutGroupName: string
  sectionName: string
  totalPoints: number
  latestCompletedAtTime: string
  selectedHaulResultId: number | null
  haulResults: HaulResultDto[]
  selectedHaulResult: HaulResultDto | null
  latestHaulResult: HaulResultDto | null
}

export type MemberCompleteDto = MemberBase & {
  scoutGroupName: string
  sectionName: string
  totalPoints: number
  latestCompletedAtTime: string
  computedImagePath: string
  clientComputedImageUri: string
  haulResults: HaulResultDto[]
  memberCompleteSummaryStatsDto: MemberCompleteSummaryStatsDto
}

export type CoinDto = {
  code: string
  baseNumber: number
  pointValue: number
}

export type ActivityBaseHaulResultDto = {
  activityBaseId: number
  activityBaseName: string
  totalPoints: number
  coinsScanned: number
  coins: CoinDto[]
}

export type HaulResultDto = {
  scavengerResultId: number
  hauledAtIso8601: string
  totalPoints: number
  activityBaseHaulResultDtos: ActivityBaseHaulResultDto[]
}

export type MemberCompleteSummaryStatsActivityBaseInfoDto = {
  names: string[]
  timesVisited: number
}

export type MemberCompleteSummaryStatsDto = {
 mostVisitedActivityBase: MemberCompleteSummaryStatsActivityBaseInfoDto
 leastVisitedActivityBase: MemberCompleteSummaryStatsActivityBaseInfoDto
 mostScans: number
 totalTokensScanned: number
}

export type ScoutGroupDto = {
  id: number
  name: string
}

export type ActivityGroupDto = {
  id: number
  name: string
}










// old ---------------------------------------------------------------------------v
export type Member = {
  firstName: string
  lastName: string
  hasImage: boolean
  memberScoutGroupNumber: number
  memberSection: string
  memberId: number
  memberCode: string
  memberNumber: number
  memberSectionName: string
  memberScoutGroupName: string
}

export type HaulResult = {
  scavengerResultId: number
  hauledAtIso8601: string
  totalPoints: number
}

export type MembersWithPoints = {
  id: number
  firstName: string
  lastName: string
  fullName: string
  hasImage: boolean
  memberCode: string
  memberNumber: number
  scoutGroupName: string
  sectionId: string
  sectionName: string
  totalPoints: number
  haulResult: HaulResult[]
  latestHaulResult?: HaulResult | undefined
  selectedHaulResultId: number | undefined
  selectedHaulResult?: HaulResult | undefined
}