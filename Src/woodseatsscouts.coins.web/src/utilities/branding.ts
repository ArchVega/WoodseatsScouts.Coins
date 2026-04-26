import type {SectionBranding} from "../types/ClientTypes.ts";

export function getSectionBranding(scoutSectionId: string): SectionBranding {
  switch (scoutSectionId.toLowerCase()) {
    case 'a': // adults
      return {foregroundColour: "black", backgroundColour: "#ffe627",}
    case 'b': // beavers
      return {foregroundColour: "white", backgroundColour: "#006ddf",}
    case 'c': // cubs
      return {foregroundColour: "white", backgroundColour: "#25b755",}
    case 'e': // explorers
      return {foregroundColour: "white", backgroundColour: "#003982",}
    case 's': // scouts
      return {foregroundColour: "white", backgroundColour: "#088486",}
    case 'n': // network
      return {foregroundColour: "white", backgroundColour: "#ff912a",}
    case 'q': // squirrels
      return {foregroundColour: "white", backgroundColour: "#ed3f23",}
    default:
      throw `No handler defined for section id '${scoutSectionId}'`
  }
}