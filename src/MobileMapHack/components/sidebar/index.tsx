import React from "react";
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
import {Journey, myJourneys} from "../../domain/types";
// @ts-ignore
import PersonImg from "../../assets/sadiqKhan.jpg";
import mainBadgeImg from "../../assets/mainpagebadge.png";

interface Section {
    title: string,
    data: Journey[]
}

const menuItems: Section[] = [
    {title: 'Pay', data: []},
    {title: 'Rewards', data: []},
    {title: 'Badges', data: []},
    {title: 'Places', data: myJourneys},
    {title: 'History', data: []},
    {title: 'Help', data: []},
    {title: 'Feedback', data: []},
    {title: 'Logout', data: []},
];

function getRouteForSection(sectionTitle: string) {
    switch(sectionTitle) {
        case 'Help':
            return 'Help';
        case 'Places':
            return 'Route';
        default:
            return 'Home';
    }
}

export const SideBar = (props: DrawerItemsProps) => {
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
                        <Image source={mainBadgeImg} style={{ width: 50, height: 50, marginTop: 10 }} />

                    </View>
                </View>
                <SectionList style={{marginLeft: 15}}
                             sections={menuItems}
                             keyExtractor={((item, index) => item + index)}
                             renderItem={({item}) =>
                                 (
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
                                 )}
                             renderSectionHeader=
                                 {({section: {title}}) => (
                                     <Text
                                        style={{padding: 10, fontWeight: 'bold'}}
                                        onPress={() => {
                                            props.navigation.closeDrawer();
                                            props.navigation.navigate(getRouteForSection(title));
                                        }}>
                                     {title}
                                     </Text>)}
                />
            </Content>
        </Container>
    )
};
