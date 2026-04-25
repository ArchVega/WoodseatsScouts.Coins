export type ScoutMemberBase = {
  id: number;
  number: number
  firstName: string
  lastName: string
  fullName: string
  scoutMemberCode: string
  scoutMemberNumber: number
  scoutGroupId: number
  scoutSectionCode: string
  computedImagePath: string
  clientComputedImageUri: string
  hasImage: boolean
}

export type ScoutMemberDto = ScoutMemberBase & {
  scoutGroupName: string
  scoutSectionName: string
  clue1State: string
  clue2State: string
  clue3State: string
  isDayVisitor: boolean
  hasImage: boolean
}

export type ScoutMemberPointsSummaryDto = ScoutMemberBase & {
  scoutGroupName: string
  scoutSectionName: string
  totalPoints: number
  latestCompletedAtTime: string
  selectedHaulResultId: number | null
  haulResults: HaulResultDto[]
  selectedHaulResult: HaulResultDto | null
  latestHaulResult: HaulResultDto | null
}

export type ScoutMemberCompleteDto = ScoutMemberBase & {
  scoutGroupName: string
  scoutSectionName: string
  totalPoints: number
  latestCompletedAtTime: string
  computedImagePath: string
  clientComputedImageUri: string
  haulResults: HaulResultDto[]
  scoutMemberCompleteSummaryStatsDto: MemberCompleteSummaryStatsDto
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

export type ScoutSectionDto = {
  code: number
  name: string
}

export type ActivityGroupDto = {
  id: number
  name: string
}