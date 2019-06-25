import React, {useState} from "react";
import {
    Content,
    Text,
    Icon,
    Container,
    View, Badge
} from "native-base";
import {Image} from "react-native";
import {DrawerItemsProps, SectionList} from "react-navigation";
import Constants from 'expo-constants';
import {Achievement, Journey, myJourneys} from "../../domain/types";
// @ts-ignore
import PersonImg from "../../assets/sadiqKhan.jpg";

interface Section {
    title: string,
    data: Journey[] | Achievement[],
    renderItem?: (data) => React.ReactElement | null;
}

function getRouteForSection(sectionTitle: string) {
    switch (sectionTitle) {
        case 'Help':
            return 'Help';
        case 'Places':
            return 'Route';
        default:
            return 'Home';
    }
}

export const SideBar = (props: DrawerItemsProps) => {

        const [openSections, setOpenSections] = useState<string[]>([]);
        const renderBadge = ({item, index, section}) => {
            if (openSections.includes(section.title)) {
                return (
                    <View style={{marginLeft: 10, padding: 10, flexDirection: 'row'}}>
                        <Text
                            style={{paddingTop: 5}}
                        >
                            {item.title}
                        </Text>
                        <Icon
                            active
                            name={item.icon}
                            type={item.iconType}
                            style={{color: item.iconColour, fontSize: 26, width: 30}}
                        />
                    </View>
                )
            } else {
                return null;
            }
        };

        const renderJourney = ({item, section}) => {
            if (openSections.includes(section.title)) {
                return (
                    <View style={{marginLeft: 10, padding: 10, flexDirection: 'row'}}>
                        <Icon
                            onPress={() => {
                                props.navigation.closeDrawer();
                                props.navigation.navigate("Route",
                                    {journey: item});
                            }}
                            active
                            type={"MaterialIcons"}
                            name={item.icon}
                            style={{color: "#777", fontSize: 26, width: 30}}
                        />
                        <Text
                            style={{paddingTop: 5}}
                            onPress={() => {
                                props.navigation.closeDrawer();
                                props.navigation.navigate("Route",
                                    {journey: item});
                            }}>
                            {item.name}
                        </Text>
                    </View>
                )
            } else {
                return null;
            }
        };

        const badges: Achievement[] = [
            {
                title: "Gold Eco",
                icon: "trophy-award" ,
                iconType: "MaterialCommunityIcons",
                iconColour: "#ffd700"
            }
        ];

        const menuItems: Section[] = [
            {title: 'Pay', data: []},
            {title: 'Rewards', data: []},
            {title: 'Badges', data: badges, renderItem: renderBadge},
            {title: 'My Journeys', data: myJourneys, renderItem: renderJourney},
            {title: 'History', data: []},
            {title: 'Help', data: []},
            {title: 'Feedback', data: []},
            {title: 'Logout', data: []},
        ];

        return (
            <Container style={{paddingTop: Constants.statusBarHeight}}>
                <Content>
                    <View style={{
                        alignSelf: 'center',
                        flexDirection: 'row',
                        alignItems: 'center',
                    }}>
                        <Image source={PersonImg} style={{width: 100, height: 100, borderRadius: 100, marginTop: 10}}/>
                        <View style={{
                            marginLeft: 15
                        }}>
                            <Text>Sadiq</Text>
                            <Text>Khan</Text>
                            <Badge success><Text>72</Text></Badge>
                        </View>
                    </View>
                    <SectionList style={{marginLeft: 15}}
                                 sections={menuItems}
                                 keyExtractor={((item, index) => item + index)}
                                 renderSectionHeader=
                                     {({section}) => (
                                         <Text
                                             style={{padding: 10, fontWeight: 'bold'}}
                                             onPress={() => {
                                                 if (section.data) {
                                                     if (openSections.includes(section.title)) {
                                                         setOpenSections(openSections.filter((s) => s !== section.title));
                                                     } else {
                                                         setOpenSections(openSections.concat(section.title));
                                                     }
                                                 } else {
                                                     props.navigation.closeDrawer();
                                                     props.navigation.navigate(getRouteForSection(section.title));
                                                 }
                                             }}>
                                             {section.title}
                                         </Text>)}
                                 renderSectionFooter={({section}) =>
                                     (section.title === "Badges" && openSections.includes(section.title)
                                         ? <Text
                                             style={{marginLeft: 10, padding: 10}}
                                         >All badges ...</Text>
                                         : null)}
                    />
                </Content>
            </Container>
        )
    }
;
